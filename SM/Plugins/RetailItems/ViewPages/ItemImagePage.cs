using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.GUI;
using LSOne.Utilities.Tools;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.RetailItems.ViewPages
{
	public partial class ItemImagePage : UserControl, ITabView
	{
		private RecordIdentifier itemId;
		private RecordIdentifier itemMasterID;
		private WeakReference owner;
		private bool isModified;
		private bool readOnly;
		private Control selectedPanel;
		private PictureBox mouseImageBox;
		private PictureBox selectedPicBox;
		private int newIndex = 10000;

		public ItemImagePage(TabControl owner)
			: this()
		{
			this.owner = new WeakReference(owner);

			readOnly = (((ViewBase)owner.Parent.Parent).ReadOnly);

			contextButtons.AddButtonEnabled = !readOnly;

			moveUpToolStripMenuItem.Image = SharedImages.ArrowUpGreen;
			moveDownToolStripMenuItem.Image = SharedImages.ArrowDownGreen;
		}

		public ItemImagePage()
		{
			isModified = false;

			InitializeComponent();

			if (components == null)
				components = new Container();
		}

		public static ITabView CreateInstance(object sender, TabControl.Tab tab)
		{
			return new ItemImagePage((TabControl)sender);
		}

		#region ITabView Members

		public bool DataIsModified()
		{
			return isModified;
		}

		public void GetAuditDescriptors(List<AuditDescriptor> contexts)
		{
			contexts.Add(new AuditDescriptor("ItemImages", itemId, Properties.Resources.Image));
		}

		public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
		{
			isModified = false;

			RetailItem item = (RetailItem)internalContext;

			itemId = item.ID;
			itemMasterID = item.MasterID;

			LoadImages();
		}

		private void LoadImages()
		{
			thumbLayout.Controls.Clear();
			pictureBox.Image = null;

			var images = Providers.RetailItemData.GetImages(PluginEntry.DataModel, itemId);
			if (images != null && images.Count > 0)
			{
				SetImage(images[0].Image, images[0]);

				foreach (var itemImage in images)
				{
					AddImage(itemImage.Image, itemImage);
				}
			}

			SetEnabledState();
		}

		private void AddImage(Image image, ItemImage itemImage)
		{
			var panel = new Panel
			{
				Size = new Size(148, 148),
				Tag = itemImage.ImageIndex,
				TabIndex = (10 + thumbLayout.Controls.Count)
			};
			panel.Paint += OnPanelPaint;
			thumbLayout.Controls.Add(panel);

			var picBox = new PictureBox
			{
				Size = new Size(144, 144),
				SizeMode = PictureBoxSizeMode.Zoom,
				Image = image,
				Tag = itemImage,
				TabIndex = (10 + thumbLayout.Controls.Count),
				Location = new Point(2, 2),
				Margin = new Padding(2)
			};
			picBox.Click += OnImageSelected;
			picBox.MouseEnter += OnPictureBoxMouseEnter;
			picBox.ContextMenuStrip = thumbContextMenuStrip;
			panel.Controls.Add(picBox);

			if (selectedPanel == null)
			{
				selectedPanel = panel;
				selectedPicBox = picBox;
			}
		}

		private void OnPictureBoxMouseEnter(object sender, EventArgs e)
		{
			mouseImageBox = sender as PictureBox;
		}

		private void OnPanelPaint(object sender, PaintEventArgs e)
		{
			var panel = (sender as Panel);
			if (panel == null || panel != selectedPanel)
				return;
			using (var P = new Pen(Color.DarkGray, 4))
			{
				P.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
				e.Graphics.DrawRectangle(P, 0, 0, panel.Width, panel.Height);
			}
		}

		private void OnImageSelected(object sender, EventArgs e)
		{
			Select(sender as PictureBox);
		}

		private void Select(PictureBox picture)
		{
			var previousBox = selectedPicBox;
			selectedPicBox = null;
			var previousPanel = selectedPanel;
			selectedPanel = null;

			if (previousBox != null)
				previousBox.Invalidate();
			if (previousPanel != null)
				previousPanel.Invalidate();

			selectedPicBox = picture;
			selectedPicBox.Invalidate();
			selectedPanel = selectedPicBox.Parent;
			selectedPanel.Invalidate();

			SetImage(selectedPicBox.Image, (ItemImage)selectedPicBox.Tag);

			SetEnabledState();
		}

		private void SetEnabledState()
		{
			if (selectedPanel == null)
			{
				moveUpToolStripMenuItem.Enabled = moveDownToolStripMenuItem.Enabled = false;
			}
			else
			{
				btnMoveUp.Enabled = moveUpToolStripMenuItem.Enabled = (selectedPanel.TabIndex > 10);
				btnMoveDown.Enabled = moveDownToolStripMenuItem.Enabled = (selectedPanel.TabIndex < (9 + thumbLayout.Controls.Count));
			}
		}

		public bool SaveData()
		{
			if (isModified)
			{
				var newList = new List<ItemImage>();
				foreach (Panel panel in thumbLayout.Controls)
				{
					var picBox = panel.Controls[0] as PictureBox;
					var itemImage = new ItemImage {
						Image = picBox.Image,
						ID = itemId,
						ItemMasterID = itemMasterID,
						ImageID = picBox.Tag != null ? ((ItemImage)picBox.Tag).ImageID : null,
						ImageName = picBox.Tag != null ? ((ItemImage)picBox.Tag).ImageName : ""
					};
					newList.Add(itemImage);
				}

				if (newList.Count > 0)
				{
					ResequenceImages(newList);
					Providers.RetailItemData.SaveImages(PluginEntry.DataModel, newList);
				}
				else
				{
					Providers.RetailItemData.DeleteImages(PluginEntry.DataModel, itemMasterID);
				}
			}

			isModified = false;

			return true;
		}

		private void ResequenceImages(List<ItemImage> itemImages)
		{
			int imageIndex = 1;
			foreach (ItemImage itemImage in itemImages)
			{
				itemImage.ImageIndex = imageIndex;
				imageIndex++;
			}
		}

		public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
		{
		}

		public void OnClose()
		{
		}

		public void SaveUserInterface()
		{
		}

		#endregion

		#region Menu handlers

		private void thumbContextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			Select(mouseImageBox);
		}

		private void contextButtons_AddButtonClicked(object sender, EventArgs e)
		{
			var dlg = new OpenFileDialog
			{
				Filter = Properties.Resources.ImageFiles + " (*.bmp;*.jpg;*.png)|*.bmp;*.jpg;*.png|" +
						 Properties.Resources.PNGfiles + " (*.png)|*.png|" +
						 Properties.Resources.JPEGfiles + " (*.jpg)|*.jpg|" +
						 Properties.Resources.BMPfiles + "  (*.bmp)|*.bmp",
                Multiselect = true
			};

			var dlgRes = dlg.ShowDialog(PluginEntry.Framework.MainWindow);
			if (dlgRes != DialogResult.Cancel)
			{
				isModified = true;

                for(int i = 0; i < dlg.FileNames.Length; i++)
                {
                    Image image = Image.FromFile(dlg.FileNames[i]);
                    ItemImage newItemImage = new ItemImage
                    {
                        Image = image,
                        ID = itemId,
                        ItemMasterID = itemMasterID,
                        ImageIndex = newIndex++,
                        ImageName = dlg.SafeFileNames[i]
                    };
                    AddImage(image, newItemImage);
                }
				
				contextButtons.RemoveButtonEnabled = true;
			}
		}

		private void moveUpToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (selectedPanel == null)
				return;
			if (selectedPanel.TabIndex > 10)
			{
				isModified = true;
				var prevControl = thumbLayout.Controls[selectedPanel.TabIndex - 1 - 10];
				thumbLayout.Controls.SetChildIndex(prevControl, selectedPanel.TabIndex - 10);
				var tmp = prevControl.TabIndex;
				prevControl.TabIndex = selectedPanel.TabIndex;
				selectedPanel.TabIndex = tmp;
				SetEnabledState();
			}
		}

		private void moveDownToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (selectedPanel == null)
				return;
			int lastIndex = 10 + thumbLayout.Controls.Count - 1;
			if (selectedPanel.TabIndex < lastIndex)
			{
				isModified = true;
				var nextControl = thumbLayout.Controls[selectedPanel.TabIndex + 1 - 10];
				thumbLayout.Controls.SetChildIndex(nextControl, selectedPanel.TabIndex - 10);
				var tmp = nextControl.TabIndex;
				nextControl.TabIndex = selectedPanel.TabIndex;
				selectedPanel.TabIndex = tmp;
				SetEnabledState();
			}
		}

		private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (selectedPanel == null)
				return;

			if (QuestionDialog.Show(Properties.Resources.DiscardImage) != DialogResult.Yes)
			{
				return;
			}
			if ((ItemImage)selectedPicBox.Tag != null)
			{
				Providers.RetailItemData.DeleteImage(PluginEntry.DataModel, (ItemImage) selectedPicBox.Tag);
			}
			isModified = true;
			thumbLayout.Controls.Remove(selectedPanel);

			selectedPanel = selectedPicBox = null;

			if (thumbLayout.Controls.Count > 0)
			{
				var panel = (Panel)thumbLayout.Controls[0];
				var picBox = panel.Controls[0] as PictureBox;
				OnImageSelected(picBox, EventArgs.Empty);
			}
			else
				SetImage(null, null);
		}

		#endregion

		private void HandleResize()
		{
			if (pictureBox != null && pictureBox.Image != null)
			{
				if (pictureBox.Image.Width > pictureBox.Width || pictureBox.Image.Height > pictureBox.Height)
					pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
				else
					pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
			}
		}

		private void OnPictureBoxSizeChanged(object sender, EventArgs args)
		{
			HandleResize();
		}

		private void SetImage(Image image, ItemImage tag)
		{
			pictureBox.Image = image;
			pictureBox.Tag = tag;
			if (image != null)
			{
				HandleResize();
				lblImageInfo.Text = string.Format("{0} x {1} [{2}]",
					image.Width,
					image.Height,
					SizeUtil.GetByteString(ImageUtils.ImageToByteArray(image).Length, 1));
				contextButtons.RemoveButtonEnabled = true;
			}
			else
				lblImageInfo.Text = "";
		}

		private void contextButtons_RemoveButtonClicked(object sender, EventArgs e)
		{
			deleteToolStripMenuItem_Click(sender, e);
		}

		private void btnMoveUp_Click(object sender, EventArgs e)
		{
			moveUpToolStripMenuItem_Click(sender, e);
		}

		private void btnMoveDown_Click(object sender, EventArgs e)
		{
			moveDownToolStripMenuItem_Click(sender, e);
		}
	}
}

