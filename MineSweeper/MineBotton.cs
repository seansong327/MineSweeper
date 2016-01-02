using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MineSweeper
{
    delegate void ButtonArroundShowStatusEventHandler(Guid thisClickTimeGuid);
    
    class MineBotton : Button
    {
        public const int widthAdHeight = 30;

        public int PositionX { get; private set; }
        public int PositionY { get; private set; }

        public bool IsMine { get; private set; }

        private int mineArroundAccount = 0;

        private Guid lastShowStatusGuid;

        private ButtonIcon buttonIcon = ButtonIcon.None;

        public event MouseEventHandler LeftMouseButtonClickEvent;
        public event ButtonArroundShowStatusEventHandler ButtonArroundShowStatusEventHandler;

        public MineBotton(int positionX, int positionY, bool isMine)
        {
            this.PositionX = positionX;
            this.PositionY = positionY;

            this.IsMine = isMine;

            this.Location = new Point(positionX * widthAdHeight, positionY * widthAdHeight);
            this.Width = widthAdHeight;
            this.Height = widthAdHeight;

            this.LeftMouseButtonClickEvent += LeftMouseButtonClick;
            this.MouseUp += this.OnClick;
        }

        public void AddMineAroundAccount()
        {
            if (!IsMine)
            {
                mineArroundAccount++;
            }
        }

        protected void OnClick(object sender, MouseEventArgs eventArg)
        {
            if (eventArg.Button == MouseButtons.Right)
            {
                if (this.Enabled)
                {
                    SetButtonIcon();
                }
            }
            else if (eventArg.Button == MouseButtons.Left)
            {
                LeftMouseButtonClickEvent(this, eventArg);
            }
        }

        private void LeftMouseButtonClick(object sender, MouseEventArgs e)
        {
            if (this.IsMine)
            {
                MessageBox.Show("Failed");
            }
            else
            {
                ShowStatus(Guid.NewGuid());
            }
        }

        private void ShowStatus(Guid thisClickTimeGuid)
        {
            if (thisClickTimeGuid != lastShowStatusGuid)
            {
                this.lastShowStatusGuid = thisClickTimeGuid;

                if (!this.IsMine && this.Enabled)
                {
                    this.Enabled = false;

                    if (this.mineArroundAccount > 0)
                    {
                        this.Text = this.mineArroundAccount.ToString();
                    }
                    else
                    {
                        ButtonArroundShowStatusEventHandler(thisClickTimeGuid);
                    }
                }
            }
        }

        public void RegisterShowStatusToButton(MineBotton button)
        {
            button.ButtonArroundShowStatusEventHandler += this.ShowStatus;
        }

        private void SetButtonIcon()
        {
            string result = null;

            ButtonIcon nextIcon = (ButtonIcon)(((int)buttonIcon + 1) % Enum.GetValues(buttonIcon.GetType()).Length);

            switch (nextIcon)
            {
                case ButtonIcon.None:
                    break;
                case ButtonIcon.Mine:
                    result = "M";
                    break;
                case ButtonIcon.UnKnown:
                    result = "U";
                    break;
            }

            buttonIcon = nextIcon;
            this.Text = result;
        }
    }
}
