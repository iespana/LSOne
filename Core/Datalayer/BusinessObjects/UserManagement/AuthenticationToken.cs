using System;
using System.ComponentModel.DataAnnotations;
using LSOne.Utilities.Cryptography;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.UserManagement
{
    public class AuthenticationToken : DataEntity
    {
        public AuthenticationToken()
            : base()
        {
            TokenID = RecordIdentifier.Empty;
            UserID = RecordIdentifier.Empty;
            TokenHash = "";            
        }

        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        public RecordIdentifier TokenID { get; set; }

        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        public RecordIdentifier UserID { get; set; }               

        [StringLength(40)]
        public string TokenHash { get; set; }

        [StringLength(40)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(TokenID, UserID);
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }

        public static string CreateHash(string rawFeed)
        {
            return HMAC_SHA1.GetValue(rawFeed, "b615ac20-6a14-11e2-bcfd-0800200c9a66");
        }

        public static AuthenticationToken FromRawFeed(RecordIdentifier tokenID, RecordIdentifier userID, string description, string rawFeed)
        {
            AuthenticationToken token = new AuthenticationToken();

            token.TokenID = tokenID;
            token.UserID = userID;
            token.Text = description;
            token.TokenHash = HMAC_SHA1.GetValue(rawFeed, "b615ac20-6a14-11e2-bcfd-0800200c9a66");

            return token;
        }
    }
}
