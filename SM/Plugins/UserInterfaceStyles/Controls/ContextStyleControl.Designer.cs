using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.UserInterfaceStyles.Controls
{
    partial class ContextStyleControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContextStyleControl));
            this.btnsEditAddRemove = new ContextButtons();
            this.lvAttributes = new ListView();
            this.column1 = new Column();
            this.colProperty = new Column();
            this.colValue1 = new Column();
            this.colError = new Column();
            this.tbParentStyle = new System.Windows.Forms.TextBox();
            this.lblParentStyle = new System.Windows.Forms.Label();
            this.addContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnEditParent = new ContextButton();
            this.SuspendLayout();
            // 
            // btnsEditAddRemove
            // 
            this.btnsEditAddRemove.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsEditAddRemove, "btnsEditAddRemove");
            this.btnsEditAddRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemove.Context = ButtonTypes.AddRemove;
            this.btnsEditAddRemove.EditButtonEnabled = false;
            this.btnsEditAddRemove.Name = "btnsEditAddRemove";
            this.btnsEditAddRemove.RemoveButtonEnabled = true;
            this.btnsEditAddRemove.AddButtonClicked += new System.EventHandler(this.btnsEditAddRemove_AddButtonClicked);
            this.btnsEditAddRemove.RemoveButtonClicked += new System.EventHandler(this.btnsEditAddRemove_RemoveButtonClicked);
            this.btnsEditAddRemove.AddButtonMouseDown += new System.Windows.Forms.MouseEventHandler(this.btnsEditAddRemove_AddButtonMouseDown);
            // 
            // lvAttributes
            // 
            resources.ApplyResources(this.lvAttributes, "lvAttributes");
            this.lvAttributes.BuddyControl = null;
            this.lvAttributes.Columns.Add(this.column1);
            this.lvAttributes.Columns.Add(this.colProperty);
            this.lvAttributes.Columns.Add(this.colValue1);
            this.lvAttributes.Columns.Add(this.colError);
            this.lvAttributes.ContentBackColor = System.Drawing.Color.White;
            this.lvAttributes.DefaultRowHeight = ((short)(18));
            this.lvAttributes.EvenRowColor = System.Drawing.Color.White;
            this.lvAttributes.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvAttributes.HeaderHeight = ((short)(20));
            this.lvAttributes.Name = "lvAttributes";
            this.lvAttributes.OddRowColor = System.Drawing.Color.White;
            this.lvAttributes.RowLineColor = System.Drawing.Color.LightGray;
            this.lvAttributes.SortSetting = "1:1";
            this.lvAttributes.SelectionChanged += new System.EventHandler(this.lvAttributes_SelectionChanged);
            this.lvAttributes.CellAction += new CellActionDelegate(this.lvAttributes_CellAction);
            this.lvAttributes.CellDropDown += new CellDropDownDelegate(this.lvAttributes_CellDropDown);
            // 
            // column1
            // 
            this.column1.AutoSize = false;
            this.column1.Clickable = false;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(10));
            this.column1.Sizable = false;
            this.column1.Tag = null;
            this.column1.Width = ((short)(20));
            // 
            // colProperty
            // 
            this.colProperty.AutoSize = true;
            this.colProperty.Clickable = false;
            this.colProperty.DefaultStyle = null;
            resources.ApplyResources(this.colProperty, "colProperty");
            this.colProperty.MaximumWidth = ((short)(0));
            this.colProperty.MinimumWidth = ((short)(10));
            this.colProperty.Sizable = false;
            this.colProperty.Tag = null;
            this.colProperty.Width = ((short)(50));
            // 
            // colValue1
            // 
            this.colValue1.AutoSize = false;
            this.colValue1.Clickable = false;
            this.colValue1.DefaultStyle = null;
            resources.ApplyResources(this.colValue1, "colValue1");
            this.colValue1.MaximumWidth = ((short)(0));
            this.colValue1.MinimumWidth = ((short)(100));
            this.colValue1.Sizable = false;
            this.colValue1.Tag = null;
            this.colValue1.Width = ((short)(300));
            // 
            // colError
            // 
            this.colError.AutoSize = false;
            this.colError.Clickable = false;
            this.colError.DefaultStyle = null;
            resources.ApplyResources(this.colError, "colError");
            this.colError.MaximumWidth = ((short)(0));
            this.colError.MinimumWidth = ((short)(10));
            this.colError.Sizable = false;
            this.colError.Tag = null;
            this.colError.Width = ((short)(44));
            // 
            // tbParentStyle
            // 
            resources.ApplyResources(this.tbParentStyle, "tbParentStyle");
            this.tbParentStyle.Name = "tbParentStyle";
            this.tbParentStyle.ReadOnly = true;
            this.tbParentStyle.TabStop = false;
            // 
            // lblParentStyle
            // 
            this.lblParentStyle.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblParentStyle, "lblParentStyle");
            this.lblParentStyle.Name = "lblParentStyle";
            // 
            // addContextMenu
            // 
            this.addContextMenu.Name = "addContextMenu";
            this.addContextMenu.ShowImageMargin = false;
            resources.ApplyResources(this.addContextMenu, "addContextMenu");
            this.addContextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.addContextMenu_ItemClicked);
            // 
            // btnEditParent
            // 
            this.btnEditParent.Context = ButtonType.Edit;
            resources.ApplyResources(this.btnEditParent, "btnEditParent");
            this.btnEditParent.Name = "btnEditParent";
            this.btnEditParent.Click += new System.EventHandler(this.btnEditParent_Click);
            // 
            // ContextStyleControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnEditParent);
            this.Controls.Add(this.tbParentStyle);
            this.Controls.Add(this.lblParentStyle);
            this.Controls.Add(this.btnsEditAddRemove);
            this.Controls.Add(this.lvAttributes);
            this.Name = "ContextStyleControl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListView lvAttributes;
        private Column colProperty;
        private Column colValue1;
        private ContextButtons btnsEditAddRemove;
        private Column column1;
        private Column colError;
        private System.Windows.Forms.TextBox tbParentStyle;
        private System.Windows.Forms.Label lblParentStyle;
        private System.Windows.Forms.ContextMenuStrip addContextMenu;
        private ContextButton btnEditParent;
    }
}
