using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MineSweeper
{
    public partial class Form1 : Form
    {
        private int buttonXCount = 20;
        private int buttonYCount = 20;
        private int mineCount = 50;

        private List<MineBotton> buttons = new List<MineBotton>();

        public Form1()
        {
            InitializeComponent();

            this.ClientSize = new Size(MineBotton.widthAdHeight * buttonXCount, MineBotton.widthAdHeight * buttonYCount);

            List<MineBotton> mineButtons = GetMineButtons(buttonXCount, buttonYCount, mineCount);

            for (int i = 0; i < buttonXCount; i++)
            {
                for (int j = 0; j < buttonYCount; j++)
                {
                    MineBotton button = mineButtons.FirstOrDefault(t => t.PositionX == i && t.PositionY == j);
                    buttons.Add(button ?? new MineBotton(i, j, false));
                }
            }

            foreach (var button in buttons)
            {
                for (int i = 0; i < 9; i++)
                {
                    int xOffset = i % 3 - 1;
                    int yOffset = i / 3 - 1;
                    if (!(xOffset == 0 && yOffset == 0))
                    {
                        RegisteAndSetMineCount(button.PositionX + xOffset, button.PositionY + yOffset, button);
                    }
                }
            }

            this.Controls.AddRange(buttons.ToArray());
        }

        private List<MineBotton> GetMineButtons(int xCount, int yCount, int mineCount)
        {
            var result = new List<MineBotton>();

            Random random = new Random();

            int x = -1;
            int y = -1;

            for (int i = 0; i < mineCount; i++)
            {
                do
                {
                    x = random.Next(xCount);
                    y = random.Next(yCount);
                } while (result.Exists(t => t.PositionX == x && t.PositionY == y));

                result.Add(new MineBotton(random.Next(xCount), random.Next(yCount), true));
            }

            return result;
        }

        private void RegisteAndSetMineCount(int x, int y, MineBotton registerToButton)
        {
            MineBotton mineButton = GetBttonByPosition(x, y);

            if (mineButton != null)
            {
                mineButton.RegisterShowStatusToButton(registerToButton);
                
                if (registerToButton.IsMine)
                {
                    mineButton.AddMineAroundAccount();
                }
            }
        }

        private MineBotton GetBttonByPosition(int x, int y)
        {
            MineBotton mineButton = null;

            if (x >= 0 && x < this.buttonXCount
                && y >= 0 && x < this.buttonYCount)
            {
                mineButton = this.buttons.FirstOrDefault(b => b.PositionX == x && b.PositionY == y);
            }

            return mineButton;
        }
    }
}
