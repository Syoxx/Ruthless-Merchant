using System;
namespace RuthlessMerchant
{
    public class DialogSystem
    {
        private DialogItem initialDialog;
        private DialogItem[] dialogs;

        public event EventHandler OnDialogOpened;
        public event EventHandler OnDialogClosed;

        public DialogItem InitialDialog
        {
            get
            {
                return initialDialog;
            }
            set
            {
                initialDialog = value;
            }
        }

        public DialogItem[] Dialogs
        {
            get
            {
                return dialogs;
            }
            set
            {
                dialogs = value;
            }
        }

        public void Open()
        {
            throw new System.NotImplementedException();
        }

        public void Close()
        {
            throw new System.NotImplementedException();
        }
    }
}