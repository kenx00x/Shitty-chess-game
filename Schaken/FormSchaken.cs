using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Schaken
{
    public partial class FormSchaken : Form
    {
        #region variabelen
        //variabelen
        int beurt = 0, WitTimerMinuten = 15, WitTimerSeconden = 0, ZwartTimerMinuten = 15, ZwartTimerSeconden = 0;
        PictureBox geselecteerdschaakstuk, geselecteerdezet, genomenstuk, PicSchaakstuk;
        PictureBox[] zetten;
        PictureBox[] schaakstukken;
        PictureBox[] schaakstukkenWit;
        PictureBox[] schaakstukkenZwart;
        bool[] PionEersteZetWit = new bool[8] { true, true, true, true, true, true, true, true };
        bool[] PionEersteZetZwart = new bool[8] { true, true, true, true, true, true, true, true };
        bool KoningEersteZetZwart = true, KoningEersteZetWit = true, schaak=false;
        bool[] TorenEersteZetWit = new bool[2] { true, true};
        bool[] TorenEersteZetZwart = new bool[2] { true, true };
        #endregion
        #region form maken
        public FormSchaken()
        {
            InitializeComponent();
            //picturebox arrays invullen
            zetten = new PictureBox[58] { Zet1,Zet2,Zet3,Zet4,Zet5,Zet6,Zet7,Zet8,Zet9, Zet10, Zet11, Zet12, Zet13, Zet14, Zet15, Zet16, Zet17, Zet18, Zet19, Zet20, Zet21, Zet22, Zet23, Zet24, Zet25, Zet26, Zet27, Zet28, Zet29, Zet30, Zet31, Zet32, Zet33, Zet34, Zet35, Zet36, Zet37, Zet38, Zet39, Zet40, Zet41, Zet42, Zet43, Zet44, Zet45, Zet46, Zet47, Zet48, Zet49, Zet50, Zet51, Zet52, Zet53, Zet54, Zet55, Zet56, ZetRokade, ZetRokade2};
            schaakstukken = new PictureBox[32] { WittePion1, WittePion2, WittePion3, WittePion4, WittePion5, WittePion6, WittePion7, WittePion8, ZwartePion1, ZwartePion2, ZwartePion3, ZwartePion4, ZwartePion5, ZwartePion6, ZwartePion7, ZwartePion8, WitteToren1, WitteToren2, ZwarteToren1, ZwarteToren2, WitteLoper1, WitteLoper2, ZwarteLoper1, ZwarteLoper2, WittePaard1, WittePaard2, ZwartePaard1, ZwartePaard2, WitteDame, ZwarteDame, WitteKoning, ZwarteKoning };
            schaakstukkenWit = new PictureBox[16] { WittePion1, WittePion2, WittePion3, WittePion4, WittePion5, WittePion6, WittePion7, WittePion8, WitteToren1, WitteToren2, WitteLoper1, WitteLoper2, WittePaard1, WittePaard2, WitteDame, WitteKoning };
            schaakstukkenZwart = new PictureBox[16] { ZwartePion1, ZwartePion2, ZwartePion3, ZwartePion4, ZwartePion5, ZwartePion6, ZwartePion7, ZwartePion8, ZwarteToren1, ZwarteToren2, ZwarteLoper1, ZwarteLoper2, ZwartePaard1, ZwartePaard2, ZwarteDame, ZwarteKoning };
            //schaakstukken juist zetten
            foreach (var item in schaakstukken)
            {
                SchaakBord.Controls.Add(item);
                item.Location = new Point(item.Location.X - 12, item.Location.Y - 12);
                item.BackColor = Color.Transparent;
            }
            //zetten juist zetten
            foreach (var item in zetten)
            {
                SchaakBord.Controls.Add(item);
            }
        }
        #endregion
        #region Objects objecten om te op te klikken
        //programma sluiten
        private void buttonSluiten_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //de gekozen zet laten werken
        private void Zet_Click(object sender, EventArgs e)
        {
            geselecteerdezet = ((PictureBox)sender);
            Zet(geselecteerdezet);
        }
        //schaakstukken klik event
        private void Schaakstuk_Click(object sender, EventArgs e)
        {
            //nakijken of je het stuk kan nemen
            if (((PictureBox)sender).BackColor == Color.FromArgb(255, 0, 0))
            {
                genomenstuk = ((PictureBox)sender);
                SchaakstukNemen(geselecteerdschaakstuk, genomenstuk);
            }
            else
            {
                geselecteerdschaakstuk = ((PictureBox)sender);
                Schaaklogica(geselecteerdschaakstuk);
            }
        }
        //zet rokade klik event
        private void ZetRokade_Click(object sender, EventArgs e)
        {
            //geselecteerd schaakstuk selecteren
            geselecteerdezet = ((PictureBox)sender);

            string kleur;
            PictureBox koning;
            schaak = false;
            if (beurt == 1)
            {
                kleur = "Witte";
                koning = ZwarteKoning;
            }
            else
            {
                kleur = "Zwarte";
                koning = WitteKoning;
            }
            if (!geselecteerdschaakstuk.Name.Contains("Koning"))
            {
                #region toren en dame nakijken
                for (int i = koning.Location.Y - 75; i >= 0; i = i - 75)
                {
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == i && element.Location.X == koning.Location.X);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Toren") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde1;
                    }
                    else if (geselecteerdezet.Location.Y == i && geselecteerdezet.Location.X == koning.Location.X)
                    {
                        goto einde1;
                    }
                }
            einde1:
                for (int i = koning.Location.Y + 75; i <= 600; i = i + 75)
                {
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == i && element.Location.X == koning.Location.X);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Toren") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde2;
                    }
                    else if (geselecteerdezet.Location.Y == i && geselecteerdezet.Location.X == koning.Location.X)
                    {
                        goto einde2;
                    }
                }
            einde2:
                for (int i = koning.Location.X + 75; i <= 600; i = i + 75)
                {
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.X == i && element.Location.Y == koning.Location.Y);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Toren") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde3;
                    }
                    else if (geselecteerdezet.Location.X == i && geselecteerdezet.Location.Y == koning.Location.Y)
                    {
                        goto einde3;
                    }
                }
            einde3:
                for (int i = koning.Location.X - 75; i >= 0; i = i - 75)
                {
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.X == i && element.Location.Y == koning.Location.Y);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Toren") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde4;
                    }
                    else if (geselecteerdezet.Location.X == i && geselecteerdezet.Location.Y == koning.Location.Y)
                    {
                        goto einde4;
                    }
                }
            einde4:;
                #endregion
                #region loper en dame nakijken
                int j = koning.Location.X;
                for (int i = koning.Location.Y - 75; i >= 0; i = i - 75)
                {
                    j = j - 75;
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == i && element.Location.X == j);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Loper") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde5;
                    }
                    else if (geselecteerdezet.Location.Y == i && geselecteerdezet.Location.X == j)
                    {
                        goto einde5;
                    }

                }
            einde5:;
                j = koning.Location.X;
                for (int i = koning.Location.Y + 75; i <= 600; i = i + 75)
                {
                    j = j - 75;
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == i && element.Location.X == j);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Loper") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde6;
                    }
                    else if (geselecteerdezet.Location.Y == i && geselecteerdezet.Location.X == j)
                    {
                        goto einde6;
                    }
                }
            einde6:;
                j = koning.Location.Y;
                for (int i = koning.Location.X + 75; i <= 600; i = i + 75)
                {
                    j = j - 75;
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.X == i && element.Location.Y == j);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Loper") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde7;
                    }
                    else if (geselecteerdezet.Location.X == i && geselecteerdezet.Location.Y == j)
                    {
                        goto einde7;
                    }
                }
            einde7:;
                j = koning.Location.Y;
                for (int i = koning.Location.X + 75; i <= 600; i = i + 75)
                {
                    j = j + 75;
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.X == i && element.Location.Y == j);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Loper") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde8;
                    }
                    else if (geselecteerdezet.Location.X == i && geselecteerdezet.Location.Y == j)
                    {
                        goto einde8;
                    }
                }
            einde8:;
                #endregion
                #region paard nakijken
                foreach (var item in schaakstukken)
                {
                    if (koning.Location.X + 75 == item.Location.X && koning.Location.Y - 150 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X - 75 == item.Location.X && koning.Location.Y - 150 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X + 75 == item.Location.X && koning.Location.Y + 150 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X - 75 == item.Location.X && koning.Location.Y + 150 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X + 150 == item.Location.X && koning.Location.Y - 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X - 150 == item.Location.X && koning.Location.Y - 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X + 150 == item.Location.X && koning.Location.Y + 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X - 150 == item.Location.X && koning.Location.Y + 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                }
                #endregion
                #region koning nakijken
                foreach (var item in schaakstukken)
                {
                    if (koning.Location.X + 75 == item.Location.X && koning.Location.Y == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X - 75 == item.Location.X && koning.Location.Y == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X + 75 == item.Location.X && koning.Location.Y + 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X - 75 == item.Location.X && koning.Location.Y + 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X == item.Location.X && koning.Location.Y - 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X == item.Location.X && koning.Location.Y + 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X - 75 == item.Location.X && koning.Location.Y - 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X + 75 == item.Location.X && koning.Location.Y - 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                }
                #endregion
                #region pion nakijken
                if (kleur == "Zwarte")
                {
                    foreach (var item in schaakstukken)
                    {
                        if (koning.Location.X == item.Location.X - 75 && koning.Location.Y == item.Location.Y + 75)
                        {
                            if (item.Name.Contains(kleur))
                            {
                                if (item.Name.Contains("Pion"))
                                {
                                    item.BackColor = Color.FromArgb(0, 0, 255);
                                    schaak = true;
                                }
                            }
                        }
                        if (koning.Location.X == item.Location.X + 75 && koning.Location.Y == item.Location.Y + 75)
                        {
                            if (item.Name.Contains(kleur))
                            {
                                if (item.Name.Contains("Pion"))
                                {
                                    item.BackColor = Color.FromArgb(0, 0, 255);
                                    schaak = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in schaakstukken)
                    {
                        if (koning.Location.X == item.Location.X - 75 && koning.Location.Y == item.Location.Y - 75)
                        {
                            if (item.Name.Contains(kleur))
                            {
                                if (item.Name.Contains("Pion"))
                                {
                                    item.BackColor = Color.FromArgb(0, 0, 255);
                                    schaak = true;
                                }
                            }
                        }
                        if (koning.Location.X == item.Location.X + 75 && koning.Location.Y == item.Location.Y - 75)
                        {
                            if (item.Name.Contains(kleur))
                            {
                                if (item.Name.Contains("Pion"))
                                {
                                    item.BackColor = Color.FromArgb(0, 0, 255);
                                    schaak = true;
                                }
                            }
                        }
                    }
                }
                #endregion
                rokadelogica();
            }
            else
            {
                #region toren en dame nakijken
                for (int i = geselecteerdezet.Location.Y - 75; i >= 0; i = i - 75)
                {
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == i && element.Location.X == geselecteerdezet.Location.X);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Toren") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde1;
                    }
                }
            einde1:
                for (int i = geselecteerdezet.Location.Y + 75; i <= 600; i = i + 75)
                {
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == i && element.Location.X == geselecteerdezet.Location.X);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Toren") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde2;
                    }
                }
            einde2:
                for (int i = geselecteerdezet.Location.X + 75; i <= 600; i = i + 75)
                {
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.X == i && element.Location.Y == geselecteerdezet.Location.Y);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Toren") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde3;
                    }
                }
            einde3:
                for (int i = geselecteerdezet.Location.X - 75; i >= 0; i = i - 75)
                {
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.X == i && element.Location.Y == geselecteerdezet.Location.Y);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Toren") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde4;
                    }
                }
            einde4:;
                #endregion
                #region loper en dame nakijken
                int j = geselecteerdezet.Location.X;
                for (int i = geselecteerdezet.Location.Y - 75; i >= 0; i = i - 75)
                {
                    j = j - 75;
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == i && element.Location.X == j);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Loper") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde5;
                    }

                }
            einde5:;
                j = geselecteerdezet.Location.X;
                for (int i = geselecteerdezet.Location.Y + 75; i <= 600; i = i + 75)
                {
                    j = j - 75;
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == i && element.Location.X == j);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Loper") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde6;
                    }
                }
            einde6:;
                j = geselecteerdezet.Location.Y;
                for (int i = geselecteerdezet.Location.X + 75; i <= 600; i = i + 75)
                {
                    j = j - 75;
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.X == i && element.Location.Y == j);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Loper") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde7;
                    }
                }
            einde7:;
                j = geselecteerdezet.Location.Y;
                for (int i = geselecteerdezet.Location.X + 75; i <= 600; i = i + 75)
                {
                    j = j + 75;
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.X == i && element.Location.Y == j);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Loper") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde8;
                    }
                }
            einde8:;
                #endregion
                #region paard nakijken
                foreach (var item in schaakstukken)
                {
                    if (geselecteerdezet.Location.X + 75 == item.Location.X && geselecteerdezet.Location.Y - 150 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (geselecteerdezet.Location.X - 75 == item.Location.X && geselecteerdezet.Location.Y - 150 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (geselecteerdezet.Location.X + 75 == item.Location.X && geselecteerdezet.Location.Y + 150 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (geselecteerdezet.Location.X - 75 == item.Location.X && geselecteerdezet.Location.Y + 150 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (geselecteerdezet.Location.X + 150 == item.Location.X && geselecteerdezet.Location.Y - 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (geselecteerdezet.Location.X - 150 == item.Location.X && geselecteerdezet.Location.Y - 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (geselecteerdezet.Location.X + 150 == item.Location.X && geselecteerdezet.Location.Y + 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (geselecteerdezet.Location.X - 150 == item.Location.X && geselecteerdezet.Location.Y + 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                }
                #endregion
                #region koning nakijken
                foreach (var item in schaakstukken)
                {
                    if (geselecteerdezet.Location.X + 75 == item.Location.X && geselecteerdezet.Location.Y == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (geselecteerdezet.Location.X - 75 == item.Location.X && geselecteerdezet.Location.Y == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (geselecteerdezet.Location.X + 75 == item.Location.X && geselecteerdezet.Location.Y + 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (geselecteerdezet.Location.X - 75 == item.Location.X && geselecteerdezet.Location.Y + 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (geselecteerdezet.Location.X == item.Location.X && geselecteerdezet.Location.Y - 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (geselecteerdezet.Location.X == item.Location.X && geselecteerdezet.Location.Y + 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (geselecteerdezet.Location.X - 75 == item.Location.X && geselecteerdezet.Location.Y - 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (geselecteerdezet.Location.X + 75 == item.Location.X && geselecteerdezet.Location.Y - 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                }
                #endregion
                #region pion nakijken
                if (kleur == "Zwarte")
                {
                    foreach (var item in schaakstukken)
                    {
                        if (geselecteerdezet.Location.X == item.Location.X - 75 && geselecteerdezet.Location.Y == item.Location.Y + 75)
                        {
                            if (item.Name.Contains(kleur))
                            {
                                if (item.Name.Contains("Pion"))
                                {
                                    item.BackColor = Color.FromArgb(0, 0, 255);
                                    schaak = true;
                                }
                            }
                        }
                        if (geselecteerdezet.Location.X == item.Location.X + 75 && geselecteerdezet.Location.Y == item.Location.Y + 75)
                        {
                            if (item.Name.Contains(kleur))
                            {
                                if (item.Name.Contains("Pion"))
                                {
                                    item.BackColor = Color.FromArgb(0, 0, 255);
                                    schaak = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in schaakstukken)
                    {
                        if (geselecteerdezet.Location.X == item.Location.X - 75 && geselecteerdezet.Location.Y == item.Location.Y - 75)
                        {
                            if (item.Name.Contains(kleur))
                            {
                                if (item.Name.Contains("Pion"))
                                {
                                    item.BackColor = Color.FromArgb(0, 0, 255);
                                    schaak = true;
                                }
                            }
                        }
                        if (geselecteerdezet.Location.X == item.Location.X + 75 && geselecteerdezet.Location.Y == item.Location.Y - 75)
                        {
                            if (item.Name.Contains(kleur))
                            {
                                if (item.Name.Contains("Pion"))
                                {
                                    item.BackColor = Color.FromArgb(0, 0, 255);
                                    schaak = true;
                                }
                            }
                        }
                    }
                }
                #endregion
                rokadelogica();
            }
        }

        private void rokadelogica()
        {
            if (schaak==false)
            {
                //schaakstuk verplaatsen
                geselecteerdschaakstuk.Location = new Point(geselecteerdezet.Location.X, geselecteerdezet.Location.Y);
                //nakijken welke rokade zet het is
                if (geselecteerdezet.Location == new Point(150, 525))
                {
                    WitteToren1.Location = new Point(geselecteerdezet.Location.X + 75, geselecteerdezet.Location.Y);
                    KoningEersteZetWit = false;
                    TorenEersteZetWit[0] = false;
                }
                else if (geselecteerdezet.Location == new Point(150, 0))
                {
                    ZwarteToren1.Location = new Point(geselecteerdezet.Location.X + 75, geselecteerdezet.Location.Y);
                    KoningEersteZetZwart = false;
                    TorenEersteZetZwart[0] = false;
                }
                else if (geselecteerdezet.Location == new Point(450, 525))
                {
                    WitteToren2.Location = new Point(geselecteerdezet.Location.X - 75, geselecteerdezet.Location.Y);
                    KoningEersteZetWit = false;
                    TorenEersteZetWit[1] = false;
                }
                else if (geselecteerdezet.Location == new Point(450, 0))
                {
                    ZwarteToren2.Location = new Point(geselecteerdezet.Location.X - 75, geselecteerdezet.Location.Y);
                    KoningEersteZetZwart = false;
                    TorenEersteZetZwart[1] = false;
                }
                beurtVeranderen();
                zettenontzichtbaar();
                achtergrondtransparent();
            }
        }
        #endregion
        #region ObjectVinden Zoeken wat soort object het is
        private void Schaaklogica(PictureBox geselecteerdschaakstuk)
        {
            achtergrondtransparent();
            zettenontzichtbaar();
            //bepalen welke kleur het schaakstuk is
            if (beurt == 0 && geselecteerdschaakstuk.Name.Substring(0,5)=="Witte")
            {
                WitteSchaakLogica(geselecteerdschaakstuk);
            }
            else if (beurt == 1 && geselecteerdschaakstuk.Name.Substring(0, 6) == "Zwarte")
            {
                ZwarteSchaakLogica(geselecteerdschaakstuk);
            }
        }
        private void ZwarteSchaakLogica(PictureBox geselecteerdschaakstuk)
        {
            //bepalen welk schaakstuk het is
            if (geselecteerdschaakstuk.Name.Contains("Pion"))
            {
                ZwartePionLogica(geselecteerdschaakstuk);
            }
            else if (geselecteerdschaakstuk.Name.Contains("Paard"))
            {
                PaardLogica(geselecteerdschaakstuk, schaakstukkenWit);
            }
            else if (geselecteerdschaakstuk.Name.Contains("Koning"))
            {
                KoningLogica(geselecteerdschaakstuk, schaakstukkenWit, KoningEersteZetZwart, TorenEersteZetZwart, ZwarteToren1, ZwarteToren2);
            }
            else if (geselecteerdschaakstuk.Name.Contains("Loper"))
            {
                LoperLogica(geselecteerdschaakstuk, "Witte");
            }
            else if (geselecteerdschaakstuk.Name.Contains("Toren"))
            {
                TorenLogica(geselecteerdschaakstuk,"Witte");
            }
            else if (geselecteerdschaakstuk.Name.Contains("Dame"))
            {
                TorenLogica(geselecteerdschaakstuk, "Witte");
                LoperLogica(geselecteerdschaakstuk, "Witte");
            }
        }
        private void WitteSchaakLogica(PictureBox geselecteerdschaakstuk)
        {
            //bepalen welk schaakstuk het is
            if (geselecteerdschaakstuk.Name.Contains("Pion"))
            {
                WittePionLogica(geselecteerdschaakstuk);
            }
            else if (geselecteerdschaakstuk.Name.Contains("Paard"))
            {
                PaardLogica(geselecteerdschaakstuk,schaakstukkenZwart);
            }
            else if (geselecteerdschaakstuk.Name.Contains("Koning"))
            {
                KoningLogica(geselecteerdschaakstuk,schaakstukkenZwart,KoningEersteZetWit, TorenEersteZetWit, WitteToren1, WitteToren2);
            }
            else if (geselecteerdschaakstuk.Name.Contains("Loper")) 
            {
                LoperLogica(geselecteerdschaakstuk, "Zwarte");
            }
            else if (geselecteerdschaakstuk.Name.Contains("Toren"))
            {
                TorenLogica(geselecteerdschaakstuk, "Zwarte");
            }
            else if (geselecteerdschaakstuk.Name.Contains("Dame"))
            {
                TorenLogica(geselecteerdschaakstuk, "Zwarte");
                LoperLogica(geselecteerdschaakstuk, "Zwarte");
            }
        }
        #endregion
        #region ObjectLogica Uitvoeren van logica van schaakstuk
        private void ZwartePionLogica(PictureBox geselecteerdschaakstuk)
        {
            //schaakstuk selecteren indien er een schaakstuk voorstaat
            PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == geselecteerdschaakstuk.Location.Y + 75 && element.Location.X == geselecteerdschaakstuk.Location.X);
            //indien er geen schaakstuk voorstaat kan het bewegen
            if (PicSchaakstuk == null)
            {
                Zet1.Location = new Point(geselecteerdschaakstuk.Location.X, geselecteerdschaakstuk.Location.Y + 75);
                Zet1.Visible = true;
                //indien hij nog geen eerste zet heeft gedaan kan hij 2 stappen zetten
                if (PionEersteZetZwart[Convert.ToInt32(geselecteerdschaakstuk.Name.Substring(geselecteerdschaakstuk.Name.Length - 1)) - 1] == true)
                {
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == geselecteerdschaakstuk.Location.Y + 150 && element.Location.X == geselecteerdschaakstuk.Location.X);
                    if (PicSchaakstuk == null)
                    {
                        Zet2.Location = new Point(geselecteerdschaakstuk.Location.X, geselecteerdschaakstuk.Location.Y + 150);
                        Zet2.Visible = true;
                    }
                }
            }
            //de 2 schuine stukken aanduiden
            foreach (var item in schaakstukkenWit)
            {
                if (geselecteerdschaakstuk.Location.X == item.Location.X - 75 && geselecteerdschaakstuk.Location.Y == item.Location.Y - 75)
                {
                    item.BackColor = Color.FromArgb(255, 0, 0);
                }
                if (geselecteerdschaakstuk.Location.X == item.Location.X + 75 && geselecteerdschaakstuk.Location.Y == item.Location.Y - 75)
                {
                    item.BackColor = Color.FromArgb(255, 0, 0);
                }
            }
        }
        private void WittePionLogica(PictureBox geselecteerdschaakstuk)
        {
            //schaakstuk selecteren indien er een schaakstuk voorstaat
            PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == geselecteerdschaakstuk.Location.Y-75 && element.Location.X == geselecteerdschaakstuk.Location.X);
            //indien er geen schaakstuk voorstaat kan het bewegen
            if (PicSchaakstuk == null)
            {
                Zet1.Location = new Point(geselecteerdschaakstuk.Location.X, geselecteerdschaakstuk.Location.Y-75);
                Zet1.Visible = true;
                //indien hij nog geen eerste zet heeft gedaan kan hij 2 stappen zetten
                if (PionEersteZetWit[Convert.ToInt32(geselecteerdschaakstuk.Name.Substring(geselecteerdschaakstuk.Name.Length - 1)) - 1] == true)
                {
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == geselecteerdschaakstuk.Location.Y - 150 && element.Location.X == geselecteerdschaakstuk.Location.X);
                    if (PicSchaakstuk == null)
                    {
                        Zet2.Location = new Point(geselecteerdschaakstuk.Location.X, geselecteerdschaakstuk.Location.Y - 150);
                        Zet2.Visible = true;
                    }
                }
            }
            //de 2 schuine stukken aanduiden
            foreach (var item in schaakstukkenZwart)
            {
                if (geselecteerdschaakstuk.Location.X == item.Location.X + 75 && geselecteerdschaakstuk.Location.Y == item.Location.Y + 75)
                {
                    item.BackColor = Color.FromArgb(255, 0, 0);
                }
                if (geselecteerdschaakstuk.Location.X == item.Location.X - 75 && geselecteerdschaakstuk.Location.Y == item.Location.Y + 75)
                {
                    item.BackColor = Color.FromArgb(255, 0, 0);
                }
            }
        }
        private void PaardLogica(PictureBox geselecteerdschaakstuk, PictureBox[] schaakstukkenKleur)
        {
            //kijken of je naar ergens kan bewegen
            PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == geselecteerdschaakstuk.Location.Y - 150 && element.Location.X == geselecteerdschaakstuk.Location.X + 75);
            if (PicSchaakstuk == null)
            {
                Zet1.Visible = true;
                Zet1.Location = new Point(geselecteerdschaakstuk.Location.X + 75, geselecteerdschaakstuk.Location.Y - 150);
            }
            PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == geselecteerdschaakstuk.Location.Y - 150 && element.Location.X == geselecteerdschaakstuk.Location.X - 75);
            if (PicSchaakstuk == null)
            {
                Zet2.Visible = true;
                Zet2.Location = new Point(geselecteerdschaakstuk.Location.X - 75, geselecteerdschaakstuk.Location.Y - 150);
            }
            PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == geselecteerdschaakstuk.Location.Y + 150 && element.Location.X == geselecteerdschaakstuk.Location.X + 75);
            if (PicSchaakstuk == null)
            {
                Zet3.Visible = true;
                Zet3.Location = new Point(geselecteerdschaakstuk.Location.X + 75, geselecteerdschaakstuk.Location.Y + 150);
            }
            PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == geselecteerdschaakstuk.Location.Y + 150 && element.Location.X == geselecteerdschaakstuk.Location.X - 75);
            if (PicSchaakstuk == null)
            {
                Zet4.Visible = true;
                Zet4.Location = new Point(geselecteerdschaakstuk.Location.X - 75, geselecteerdschaakstuk.Location.Y + 150);
            }
            PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == geselecteerdschaakstuk.Location.Y - 75 && element.Location.X == geselecteerdschaakstuk.Location.X + 150);
            if (PicSchaakstuk == null)
            {
                Zet5.Visible = true;
                Zet5.Location = new Point(geselecteerdschaakstuk.Location.X + 150, geselecteerdschaakstuk.Location.Y - 75);
            }
            PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == geselecteerdschaakstuk.Location.Y - 75 && element.Location.X == geselecteerdschaakstuk.Location.X - 150);
            if (PicSchaakstuk == null)
            {
                Zet6.Visible = true;
                Zet6.Location = new Point(geselecteerdschaakstuk.Location.X - 150, geselecteerdschaakstuk.Location.Y - 75);
            }
            PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == geselecteerdschaakstuk.Location.Y + 75 && element.Location.X == geselecteerdschaakstuk.Location.X + 150);
            if (PicSchaakstuk == null)
            {
                Zet7.Visible = true;
                Zet7.Location = new Point(geselecteerdschaakstuk.Location.X + 150, geselecteerdschaakstuk.Location.Y + 75);
            }
            PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == geselecteerdschaakstuk.Location.Y + 75 && element.Location.X == geselecteerdschaakstuk.Location.X - 150);
            if (PicSchaakstuk == null)
            {
                Zet8.Visible = true;
                Zet8.Location = new Point(geselecteerdschaakstuk.Location.X - 150, geselecteerdschaakstuk.Location.Y + 75);
            }
            //kijken of je een stuk kan nemen van de andere kleur
            foreach (var item in schaakstukkenKleur)
            {
                if (geselecteerdschaakstuk.Location.X + 75 == item.Location.X && geselecteerdschaakstuk.Location.Y - 150 == item.Location.Y)
                {
                    item.BackColor = Color.FromArgb(255, 0, 0);
                }
                if (geselecteerdschaakstuk.Location.X - 75 == item.Location.X && geselecteerdschaakstuk.Location.Y - 150 == item.Location.Y)
                {
                    item.BackColor = Color.FromArgb(255, 0, 0);
                }
                if (geselecteerdschaakstuk.Location.X + 75 == item.Location.X && geselecteerdschaakstuk.Location.Y + 150 == item.Location.Y)
                {
                    item.BackColor = Color.FromArgb(255, 0, 0);
                }
                if (geselecteerdschaakstuk.Location.X - 75 == item.Location.X && geselecteerdschaakstuk.Location.Y + 150 == item.Location.Y)
                {
                    item.BackColor = Color.FromArgb(255, 0, 0);
                }
                if (geselecteerdschaakstuk.Location.X + 150 == item.Location.X && geselecteerdschaakstuk.Location.Y - 75 == item.Location.Y)
                {
                    item.BackColor = Color.FromArgb(255, 0, 0);
                }
                if (geselecteerdschaakstuk.Location.X - 150 == item.Location.X && geselecteerdschaakstuk.Location.Y - 75 == item.Location.Y)
                {
                    item.BackColor = Color.FromArgb(255, 0, 0);
                }
                if (geselecteerdschaakstuk.Location.X + 150 == item.Location.X && geselecteerdschaakstuk.Location.Y + 75 == item.Location.Y)
                {
                    item.BackColor = Color.FromArgb(255, 0, 0);
                }
                if (geselecteerdschaakstuk.Location.X - 150 == item.Location.X && geselecteerdschaakstuk.Location.Y + 75 == item.Location.Y)
                {
                    item.BackColor = Color.FromArgb(255, 0, 0);
                }
            }
        }
        private void KoningLogica(PictureBox geselecteerdschaakstuk, PictureBox[] schaakstukkenKleur, bool KoningEersteZetKleur, bool[] TorenEersteZetKleur, PictureBox toren1, PictureBox toren2)
        {
            if (schaak == false)
            {
                //nakijken dat de koning en de toren nog niet bewogen hebben om rokade te kunnen doen
                if (KoningEersteZetKleur == true && TorenEersteZetKleur[0] == true)
                {
                    PictureBox schaakstuk1 = Array.Find(schaakstukken, element => element.Location.Y == geselecteerdschaakstuk.Location.Y && element.Location.X == geselecteerdschaakstuk.Location.X - 75);
                    PictureBox schaakstuk2 = Array.Find(schaakstukken, element => element.Location.Y == geselecteerdschaakstuk.Location.Y && element.Location.X == geselecteerdschaakstuk.Location.X - 150);
                    PictureBox schaakstuk3 = Array.Find(schaakstukken, element => element.Location.Y == geselecteerdschaakstuk.Location.Y && element.Location.X == geselecteerdschaakstuk.Location.X - 225);
                    PictureBox schaakstuk4 = Array.Find(schaakstukken, element => element.Location.Y == geselecteerdschaakstuk.Location.Y && element.Location.X == geselecteerdschaakstuk.Location.X - 300);
                    if (schaakstuk1 == null && schaakstuk2 == null && schaakstuk3 == null && schaakstuk4 == toren1)
                    {
                        ZetRokade.Visible = true;
                        ZetRokade.Location = new Point(geselecteerdschaakstuk.Location.X - 150, geselecteerdschaakstuk.Location.Y);
                    }
                }
                if (KoningEersteZetKleur == true && TorenEersteZetKleur[1] == true)
                {
                    PictureBox schaakstuk1 = Array.Find(schaakstukken, element => element.Location.Y == geselecteerdschaakstuk.Location.Y && element.Location.X == geselecteerdschaakstuk.Location.X + 75);
                    PictureBox schaakstuk2 = Array.Find(schaakstukken, element => element.Location.Y == geselecteerdschaakstuk.Location.Y && element.Location.X == geselecteerdschaakstuk.Location.X + 150);
                    PictureBox schaakstuk3 = Array.Find(schaakstukken, element => element.Location.Y == geselecteerdschaakstuk.Location.Y && element.Location.X == geselecteerdschaakstuk.Location.X + 225);
                    if (schaakstuk1 == null && schaakstuk2 == null && schaakstuk3 == toren2)
                    {
                        ZetRokade2.Visible = true;
                        ZetRokade2.Location = new Point(geselecteerdschaakstuk.Location.X + 150, geselecteerdschaakstuk.Location.Y);
                    }
                }
            }
            //controleren of je kan bewegen
            PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == geselecteerdschaakstuk.Location.Y && element.Location.X == geselecteerdschaakstuk.Location.X + 75);
            if (PicSchaakstuk == null)
            {
                Zet1.Visible = true;
                Zet1.Location = new Point(geselecteerdschaakstuk.Location.X + 75, geselecteerdschaakstuk.Location.Y);
            }
            PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == geselecteerdschaakstuk.Location.Y && element.Location.X == geselecteerdschaakstuk.Location.X - 75);
            if (PicSchaakstuk == null)
            {
                Zet2.Visible = true;
                Zet2.Location = new Point(geselecteerdschaakstuk.Location.X - 75, geselecteerdschaakstuk.Location.Y);
            }
            PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == geselecteerdschaakstuk.Location.Y + 75 && element.Location.X == geselecteerdschaakstuk.Location.X + 75);
            if (PicSchaakstuk == null)
            {
                Zet3.Visible = true;
                Zet3.Location = new Point(geselecteerdschaakstuk.Location.X + 75, geselecteerdschaakstuk.Location.Y + 75);
            }
            PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == geselecteerdschaakstuk.Location.Y + 75 && element.Location.X == geselecteerdschaakstuk.Location.X - 75);
            if (PicSchaakstuk == null)
            {
                Zet4.Visible = true;
                Zet4.Location = new Point(geselecteerdschaakstuk.Location.X - 75, geselecteerdschaakstuk.Location.Y + 75);
            }
            PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == geselecteerdschaakstuk.Location.Y - 75 && element.Location.X == geselecteerdschaakstuk.Location.X);
            if (PicSchaakstuk == null)
            {
                Zet5.Visible = true;
                Zet5.Location = new Point(geselecteerdschaakstuk.Location.X, geselecteerdschaakstuk.Location.Y - 75);
            }
            PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == geselecteerdschaakstuk.Location.Y + 75 && element.Location.X == geselecteerdschaakstuk.Location.X);
            if (PicSchaakstuk == null)
            {
                Zet6.Visible = true;
                Zet6.Location = new Point(geselecteerdschaakstuk.Location.X, geselecteerdschaakstuk.Location.Y + 75);
            }
            PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == geselecteerdschaakstuk.Location.Y - 75 && element.Location.X == geselecteerdschaakstuk.Location.X + 75);
            if (PicSchaakstuk == null)
            {
                Zet7.Visible = true;
                Zet7.Location = new Point(geselecteerdschaakstuk.Location.X + 75, geselecteerdschaakstuk.Location.Y - 75);
            }
            PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == geselecteerdschaakstuk.Location.Y - 75 && element.Location.X == geselecteerdschaakstuk.Location.X - 75);
            if (PicSchaakstuk == null)
            {
                Zet8.Visible = true;
                Zet8.Location = new Point(geselecteerdschaakstuk.Location.X - 75, geselecteerdschaakstuk.Location.Y - 75);
            }
            //controleren of er iets genomen kan worden
            foreach (var item in schaakstukkenKleur)
            {
                if (geselecteerdschaakstuk.Location.X + 75 == item.Location.X && geselecteerdschaakstuk.Location.Y == item.Location.Y)
                {
                    item.BackColor = Color.FromArgb(255, 0, 0);
                }
                if (geselecteerdschaakstuk.Location.X - 75 == item.Location.X && geselecteerdschaakstuk.Location.Y == item.Location.Y)
                {
                    item.BackColor = Color.FromArgb(255, 0, 0);
                }
                if (geselecteerdschaakstuk.Location.X + 75 == item.Location.X && geselecteerdschaakstuk.Location.Y + 75 == item.Location.Y)
                {
                    item.BackColor = Color.FromArgb(255, 0, 0);
                }
                if (geselecteerdschaakstuk.Location.X - 75 == item.Location.X && geselecteerdschaakstuk.Location.Y + 75 == item.Location.Y)
                {
                    item.BackColor = Color.FromArgb(255, 0, 0);
                }
                if (geselecteerdschaakstuk.Location.X == item.Location.X && geselecteerdschaakstuk.Location.Y - 75 == item.Location.Y)
                {
                    item.BackColor = Color.FromArgb(255, 0, 0);
                }
                if (geselecteerdschaakstuk.Location.X == item.Location.X && geselecteerdschaakstuk.Location.Y + 75 == item.Location.Y)
                {
                    item.BackColor = Color.FromArgb(255, 0, 0);
                }
                if (geselecteerdschaakstuk.Location.X - 75 == item.Location.X && geselecteerdschaakstuk.Location.Y - 75 == item.Location.Y)
                {
                    item.BackColor = Color.FromArgb(255, 0, 0);
                }
                if (geselecteerdschaakstuk.Location.X + 75 == item.Location.X && geselecteerdschaakstuk.Location.Y - 75 == item.Location.Y)
                {
                    item.BackColor = Color.FromArgb(255, 0, 0);
                }
            }
        }
        private void TorenLogica(PictureBox geselecteerdschaakstuk, string Kleur)
        {
            //elk vakje in een bepaalde richting nakijken voor een schaakstuk tot er een schaakstuk is
            int plaatsen = 0;
            int schaakstuk = 0;
            for (int i = geselecteerdschaakstuk.Location.Y - 75; i >= 0; i = i - 75)
            {
                plaatsen++;
                schaakstuk++;
                PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == i && element.Location.X == geselecteerdschaakstuk.Location.X);
                if (PicSchaakstuk != null)
                {
                    if (PicSchaakstuk.Name.Contains(Kleur))
                    {
                        PicSchaakstuk.BackColor = Color.FromArgb(255, 0, 0);
                    }
                    goto einde1;
                }
                else
                {
                    zetten[schaakstuk - 1].Visible = true;
                    zetten[schaakstuk - 1].Location = new Point(geselecteerdschaakstuk.Location.X, geselecteerdschaakstuk.Location.Y - (75 * plaatsen));
                }
            }
            einde1:;
            plaatsen = 0;
            schaakstuk = 7;
            for (int i = geselecteerdschaakstuk.Location.Y + 75; i <= 600; i = i + 75)
            {
                plaatsen++;
                schaakstuk++;
                PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == i && element.Location.X == geselecteerdschaakstuk.Location.X);
                if (PicSchaakstuk != null)
                {
                    if (PicSchaakstuk.Name.Contains(Kleur))
                    {
                        PicSchaakstuk.BackColor = Color.FromArgb(255, 0, 0);
                    }
                    goto einde2;
                }
                else
                {
                    zetten[schaakstuk - 1].Visible = true;
                    zetten[schaakstuk - 1].Location = new Point(geselecteerdschaakstuk.Location.X, geselecteerdschaakstuk.Location.Y + (75 * plaatsen));
                }
            }
            einde2:;
            plaatsen = 0;
            schaakstuk = 14;
            for (int i = geselecteerdschaakstuk.Location.X + 75; i <= 600; i = i + 75)
            {
                plaatsen++;
                schaakstuk++;
                PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.X == i && element.Location.Y == geselecteerdschaakstuk.Location.Y);
                if (PicSchaakstuk != null)
                {
                    if (PicSchaakstuk.Name.Contains(Kleur))
                    {
                        PicSchaakstuk.BackColor = Color.FromArgb(255, 0, 0);
                    }
                    goto einde3;
                }
                else
                {
                    zetten[schaakstuk - 1].Visible = true;
                    zetten[schaakstuk - 1].Location = new Point(geselecteerdschaakstuk.Location.X + (75 * plaatsen), geselecteerdschaakstuk.Location.Y);
                }
            }
            einde3:;
            plaatsen = 0;
            schaakstuk = 21;
            for (int i = geselecteerdschaakstuk.Location.X - 75; i >= 0; i = i - 75)
            {
                plaatsen++;
                schaakstuk++;
                PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.X == i && element.Location.Y == geselecteerdschaakstuk.Location.Y);
                if (PicSchaakstuk != null)
                {
                    if (PicSchaakstuk.Name.Contains(Kleur))
                    {
                        PicSchaakstuk.BackColor = Color.FromArgb(255, 0, 0);
                    }
                    goto einde4;
                }
                else
                {
                    zetten[schaakstuk - 1].Visible = true;
                    zetten[schaakstuk - 1].Location = new Point(geselecteerdschaakstuk.Location.X - (75 * plaatsen), geselecteerdschaakstuk.Location.Y);
                }
            }
            einde4:;
        }
        private void LoperLogica(PictureBox geselecteerdschaakstuk, string Kleur)
        {
            //elk vakje in een bepaalde richting nakijken voor een schaakstuk tot er een schaakstuk is
            int plaatsen = 0;
            int schaakstuk = 28;
            int j = geselecteerdschaakstuk.Location.X;
            for (int i = geselecteerdschaakstuk.Location.Y - 75; i >= 0; i = i - 75)
            {
                j = j - 75;
                plaatsen++;
                schaakstuk++;
                PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == i && element.Location.X == j);
                if (PicSchaakstuk != null)
                {
                    if (PicSchaakstuk.Name.Contains(Kleur))
                    {
                        PicSchaakstuk.BackColor = Color.FromArgb(255, 0, 0);
                    }
                    goto einde5;
                }
                else
                {
                    zetten[schaakstuk - 1].Visible = true;
                    zetten[schaakstuk - 1].Location = new Point(geselecteerdschaakstuk.Location.X - (75 * plaatsen), geselecteerdschaakstuk.Location.Y - (75 * plaatsen));
                }

            }
            einde5:;
            plaatsen = 0;
            schaakstuk = 35;
            j = geselecteerdschaakstuk.Location.X;
            for (int i = geselecteerdschaakstuk.Location.Y + 75; i <= 600; i = i + 75)
            {
                j = j - 75;
                plaatsen++;
                schaakstuk++;
                PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == i && element.Location.X == j);
                if (PicSchaakstuk != null)
                {
                    if (PicSchaakstuk.Name.Contains(Kleur))
                    {
                        PicSchaakstuk.BackColor = Color.FromArgb(255, 0, 0);
                    }
                    goto einde6;
                }
                else
                {
                    zetten[schaakstuk - 1].Visible = true;
                    zetten[schaakstuk - 1].Location = new Point(geselecteerdschaakstuk.Location.X - (75 * plaatsen), geselecteerdschaakstuk.Location.Y + (75 * plaatsen));
                }
            }
            einde6:;
            plaatsen = 0;
            schaakstuk = 42;
            j = geselecteerdschaakstuk.Location.Y;
            for (int i = geselecteerdschaakstuk.Location.X + 75; i <= 600; i = i + 75)
            {
                j = j - 75;
                plaatsen++;
                schaakstuk++;
                PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.X == i && element.Location.Y == j);
                if (PicSchaakstuk != null)
                {
                    if (PicSchaakstuk.Name.Contains(Kleur))
                    {
                        PicSchaakstuk.BackColor = Color.FromArgb(255, 0, 0);
                    }
                    goto einde7;
                }
                else
                {
                    zetten[schaakstuk - 1].Visible = true;
                    zetten[schaakstuk - 1].Location = new Point(geselecteerdschaakstuk.Location.X + (75 * plaatsen), geselecteerdschaakstuk.Location.Y - (75 * plaatsen));
                }
            }
            einde7:;
            plaatsen = 0;
            schaakstuk = 49;
            j = geselecteerdschaakstuk.Location.Y;
            for (int i = geselecteerdschaakstuk.Location.X + 75; i <= 600; i = i + 75)
            {
                j = j + 75;
                plaatsen++;
                schaakstuk++;
                PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.X == i && element.Location.Y == j);
                if (PicSchaakstuk != null)
                {
                    if (PicSchaakstuk.Name.Contains(Kleur))
                    {
                        PicSchaakstuk.BackColor = Color.FromArgb(255, 0, 0);
                    }
                    goto einde8;
                }
                else
                {
                    zetten[schaakstuk - 1].Visible = true;
                    zetten[schaakstuk - 1].Location = new Point(geselecteerdschaakstuk.Location.X + (75 * plaatsen), geselecteerdschaakstuk.Location.Y + (75 * plaatsen));
                }
            }
            einde8:;
        }


        #region Uitvoeren van zet
        private void Zet(PictureBox geselecteerdezet)
        {
            string kleur;
            PictureBox koning;
            schaak = false;
            if (beurt == 1)
            {
                kleur = "Witte";
                koning = ZwarteKoning;
            }
            else
            {
                kleur = "Zwarte";
                koning = WitteKoning;
            }
            if (!geselecteerdschaakstuk.Name.Contains("Koning"))
            {
                #region toren en dame nakijken
                for (int i = koning.Location.Y - 75; i >= 0; i = i - 75)
                {
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == i && element.Location.X == koning.Location.X);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Toren") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde1;
                    }
                    else if (geselecteerdezet.Location.Y == i && geselecteerdezet.Location.X == koning.Location.X)
                    {
                        goto einde1;
                    }
                }
            einde1:
                for (int i = koning.Location.Y + 75; i <= 600; i = i + 75)
                {
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == i && element.Location.X == koning.Location.X);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Toren") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde2;
                    }
                    else if (geselecteerdezet.Location.Y == i && geselecteerdezet.Location.X == koning.Location.X)
                    {
                        goto einde2;
                    }
                }
            einde2:
                for (int i = koning.Location.X + 75; i <= 600; i = i + 75)
                {
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.X == i && element.Location.Y == koning.Location.Y);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Toren") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde3;
                    }
                    else if (geselecteerdezet.Location.X == i && geselecteerdezet.Location.Y == koning.Location.Y)
                    {
                        goto einde3;
                    }
                }
            einde3:
                for (int i = koning.Location.X - 75; i >= 0; i = i - 75)
                {
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.X == i && element.Location.Y == koning.Location.Y);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Toren") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde4;
                    }
                    else if (geselecteerdezet.Location.X == i && geselecteerdezet.Location.Y == koning.Location.Y)
                    {
                        goto einde4;
                    }
                }
            einde4:;
                #endregion
                #region loper en dame nakijken
                int j = koning.Location.X;
                for (int i = koning.Location.Y - 75; i >= 0; i = i - 75)
                {
                    j = j - 75;
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == i && element.Location.X == j);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Loper") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde5;
                    }
                    else if (geselecteerdezet.Location.Y == i && geselecteerdezet.Location.X == j)
                    {
                        goto einde5;
                    }

                }
            einde5:;
                j = koning.Location.X;
                for (int i = koning.Location.Y + 75; i <= 600; i = i + 75)
                {
                    j = j - 75;
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == i && element.Location.X == j);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Loper") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde6;
                    }
                    else if (geselecteerdezet.Location.Y == i && geselecteerdezet.Location.X == j)
                    {
                        goto einde6;
                    }
                }
            einde6:;
                j = koning.Location.Y;
                for (int i = koning.Location.X + 75; i <= 600; i = i + 75)
                {
                    j = j - 75;
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.X == i && element.Location.Y == j);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Loper") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde7;
                    }
                    else if (geselecteerdezet.Location.X == i && geselecteerdezet.Location.Y == j)
                    {
                        goto einde7;
                    }
                }
            einde7:;
                j = koning.Location.Y;
                for (int i = koning.Location.X + 75; i <= 600; i = i + 75)
                {
                    j = j + 75;
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.X == i && element.Location.Y == j);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Loper") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde8;
                    }
                    else if (geselecteerdezet.Location.X == i && geselecteerdezet.Location.Y == j)
                    {
                        goto einde8;
                    }
                }
            einde8:;
                #endregion
                #region paard nakijken
                foreach (var item in schaakstukken)
                {
                    if (koning.Location.X + 75 == item.Location.X && koning.Location.Y - 150 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X - 75 == item.Location.X && koning.Location.Y - 150 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X + 75 == item.Location.X && koning.Location.Y + 150 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X - 75 == item.Location.X && koning.Location.Y + 150 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X + 150 == item.Location.X && koning.Location.Y - 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X - 150 == item.Location.X && koning.Location.Y - 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X + 150 == item.Location.X && koning.Location.Y + 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X - 150 == item.Location.X && koning.Location.Y + 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                }
                #endregion
                #region koning nakijken
                foreach (var item in schaakstukken)
                {
                    if (koning.Location.X + 75 == item.Location.X && koning.Location.Y == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X - 75 == item.Location.X && koning.Location.Y == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X + 75 == item.Location.X && koning.Location.Y + 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X - 75 == item.Location.X && koning.Location.Y + 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X == item.Location.X && koning.Location.Y - 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X == item.Location.X && koning.Location.Y + 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X - 75 == item.Location.X && koning.Location.Y - 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X + 75 == item.Location.X && koning.Location.Y - 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                }
                #endregion
                #region pion nakijken
                if (kleur == "Zwarte")
                {
                    foreach (var item in schaakstukken)
                    {
                        if (koning.Location.X == item.Location.X - 75 && koning.Location.Y == item.Location.Y + 75)
                        {
                            if (item.Name.Contains(kleur))
                            {
                                if (item.Name.Contains("Pion"))
                                {
                                    item.BackColor = Color.FromArgb(0, 0, 255);
                                    schaak = true;
                                }
                            }
                        }
                        if (koning.Location.X == item.Location.X + 75 && koning.Location.Y == item.Location.Y + 75)
                        {
                            if (item.Name.Contains(kleur))
                            {
                                if (item.Name.Contains("Pion"))
                                {
                                    item.BackColor = Color.FromArgb(0, 0, 255);
                                    schaak = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in schaakstukken)
                    {
                        if (koning.Location.X == item.Location.X - 75 && koning.Location.Y == item.Location.Y - 75)
                        {
                            if (item.Name.Contains(kleur))
                            {
                                if (item.Name.Contains("Pion"))
                                {
                                    item.BackColor = Color.FromArgb(0, 0, 255);
                                    schaak = true;
                                }
                            }
                        }
                        if (koning.Location.X == item.Location.X + 75 && koning.Location.Y == item.Location.Y - 75)
                        {
                            if (item.Name.Contains(kleur))
                            {
                                if (item.Name.Contains("Pion"))
                                {
                                    item.BackColor = Color.FromArgb(0, 0, 255);
                                    schaak = true;
                                }
                            }
                        }
                    }
                }
                #endregion
                Zetlogica();
            }
            else
            {
                #region toren en dame nakijken
                for (int i = geselecteerdezet.Location.Y - 75; i >= 0; i = i - 75)
                {
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == i && element.Location.X == geselecteerdezet.Location.X);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Toren") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak=true;
                            }
                        }
                        goto einde1;
                    }
                }
            einde1:
                for (int i = geselecteerdezet.Location.Y + 75; i <= 600; i = i + 75)
                {
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == i && element.Location.X == geselecteerdezet.Location.X);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Toren") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak=true;
                            }
                        }
                        goto einde2;
                    }
                }
            einde2:
                for (int i = geselecteerdezet.Location.X + 75; i <= 600; i = i + 75)
                {
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.X == i && element.Location.Y == geselecteerdezet.Location.Y);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Toren") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak=true;
                            }
                        }
                        goto einde3;
                    }
                }
            einde3:
                for (int i = geselecteerdezet.Location.X - 75; i >= 0; i = i - 75)
                {
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.X == i && element.Location.Y == geselecteerdezet.Location.Y);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Toren") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak=true;
                            }
                        }
                        goto einde4;
                    }
                }
            einde4:;
                #endregion
                #region loper en dame nakijken
                int j = geselecteerdezet.Location.X;
                for (int i = geselecteerdezet.Location.Y - 75; i >= 0; i = i - 75)
                {
                    j = j - 75;
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == i && element.Location.X == j);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Loper") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak=true;
                            }
                        }
                        goto einde5;
                    }

                }
            einde5:;
                j = geselecteerdezet.Location.X;
                for (int i = geselecteerdezet.Location.Y + 75; i <= 600; i = i + 75)
                {
                    j = j - 75;
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == i && element.Location.X == j);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Loper") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak=true;
                            }
                        }
                        goto einde6;
                    }
                }
            einde6:;
                j = geselecteerdezet.Location.Y;
                for (int i = geselecteerdezet.Location.X + 75; i <= 600; i = i + 75)
                {
                    j = j - 75;
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.X == i && element.Location.Y == j);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Loper") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak=true;
                            }
                        }
                        goto einde7;
                    }
                }
            einde7:;
                j = geselecteerdezet.Location.Y;
                for (int i = geselecteerdezet.Location.X + 75; i <= 600; i = i + 75)
                {
                    j = j + 75;
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.X == i && element.Location.Y == j);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Loper") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak=true;
                            }
                        }
                        goto einde8;
                    }
                }
            einde8:;
                #endregion
                #region paard nakijken
                foreach (var item in schaakstukken)
                {
                    if (geselecteerdezet.Location.X + 75 == item.Location.X && geselecteerdezet.Location.Y - 150 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak=true;
                            }
                        }
                    }
                    if (geselecteerdezet.Location.X - 75 == item.Location.X && geselecteerdezet.Location.Y - 150 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak=true;
                            }
                        }
                    }
                    if (geselecteerdezet.Location.X + 75 == item.Location.X && geselecteerdezet.Location.Y + 150 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak=true;
                            }
                        }
                    }
                    if (geselecteerdezet.Location.X - 75 == item.Location.X && geselecteerdezet.Location.Y + 150 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak=true;
                            }
                        }
                    }
                    if (geselecteerdezet.Location.X + 150 == item.Location.X && geselecteerdezet.Location.Y - 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak=true;
                            }
                        }
                    }
                    if (geselecteerdezet.Location.X - 150 == item.Location.X && geselecteerdezet.Location.Y - 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak=true;
                            }
                        }
                    }
                    if (geselecteerdezet.Location.X + 150 == item.Location.X && geselecteerdezet.Location.Y + 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak=true;
                            }
                        }
                    }
                    if (geselecteerdezet.Location.X - 150 == item.Location.X && geselecteerdezet.Location.Y + 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak=true;
                            }
                        }
                    }
                }
                #endregion
                #region koning nakijken
                foreach (var item in schaakstukken)
                {
                    if (geselecteerdezet.Location.X + 75 == item.Location.X && geselecteerdezet.Location.Y == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak=true;
                            }
                        }
                    }
                    if (geselecteerdezet.Location.X - 75 == item.Location.X && geselecteerdezet.Location.Y == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak=true;
                            }
                        }
                    }
                    if (geselecteerdezet.Location.X + 75 == item.Location.X && geselecteerdezet.Location.Y + 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak=true;
                            }
                        }
                    }
                    if (geselecteerdezet.Location.X - 75 == item.Location.X && geselecteerdezet.Location.Y + 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak=true;
                            }
                        }
                    }
                    if (geselecteerdezet.Location.X == item.Location.X && geselecteerdezet.Location.Y - 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak=true;
                            }
                        }
                    }
                    if (geselecteerdezet.Location.X == item.Location.X && geselecteerdezet.Location.Y + 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak=true;
                            }
                        }
                    }
                    if (geselecteerdezet.Location.X - 75 == item.Location.X && geselecteerdezet.Location.Y - 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak=true;
                            }
                        }
                    }
                    if (geselecteerdezet.Location.X + 75 == item.Location.X && geselecteerdezet.Location.Y - 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak=true;
                            }
                        }
                    }
                }
                #endregion
                #region pion nakijken
                if (kleur == "Zwarte")
                {
                    foreach (var item in schaakstukken)
                    {
                        if (geselecteerdezet.Location.X == item.Location.X - 75 && geselecteerdezet.Location.Y == item.Location.Y + 75)
                        {
                            if (item.Name.Contains(kleur))
                            {
                                if (item.Name.Contains("Pion"))
                                {
                                    item.BackColor = Color.FromArgb(0, 0, 255);
                                    schaak=true;
                                }
                            }
                        }
                        if (geselecteerdezet.Location.X == item.Location.X + 75 && geselecteerdezet.Location.Y == item.Location.Y + 75)
                        {
                            if (item.Name.Contains(kleur))
                            {
                                if (item.Name.Contains("Pion"))
                                {
                                    item.BackColor = Color.FromArgb(0, 0, 255);
                                    schaak=true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in schaakstukken)
                    {
                        if (geselecteerdezet.Location.X == item.Location.X - 75 && geselecteerdezet.Location.Y == item.Location.Y - 75)
                        {
                            if (item.Name.Contains(kleur))
                            {
                                if (item.Name.Contains("Pion"))
                                {
                                    item.BackColor = Color.FromArgb(0, 0, 255);
                                    schaak=true;
                                }
                            }
                        }
                        if (geselecteerdezet.Location.X == item.Location.X + 75 && geselecteerdezet.Location.Y == item.Location.Y - 75)
                        {
                            if (item.Name.Contains(kleur))
                            {
                                if (item.Name.Contains("Pion"))
                                {
                                    item.BackColor = Color.FromArgb(0, 0, 255);
                                    schaak=true;
                                }
                            }
                        }
                    }
                }
                #endregion
                Zetlogica();
            }
        }
        private void Zetlogica()
        {
            if (schaak == false)
            {
                achtergrondtransparent();
                eerstezet();
                zettenontzichtbaar();
                //locatie schaakstuk veranderen
                geselecteerdschaakstuk.Location = new Point(geselecteerdezet.Location.X, geselecteerdezet.Location.Y);
                //Witte pion veranderen in witte dame wanneer hij aan de overkant geraakt
                if (geselecteerdschaakstuk.Name.Substring(0, geselecteerdschaakstuk.Name.Length - 1) == "WittePion" && geselecteerdschaakstuk.Location.Y == 0)
                {
                    geselecteerdschaakstuk.Name = "WitteDame" + geselecteerdschaakstuk.Name.Substring(geselecteerdschaakstuk.Name.Length - 1);
                    geselecteerdschaakstuk.BackgroundImage = WitteDame.BackgroundImage;
                }
                //Zwarte pion veranderen in Zwarte dame wanneer hij aan de overkant geraakt
                if (geselecteerdschaakstuk.Name.Substring(0, geselecteerdschaakstuk.Name.Length - 1) == "ZwartePion" && geselecteerdschaakstuk.Location.Y == 525)
                {
                    geselecteerdschaakstuk.Name = "ZwarteDame" + geselecteerdschaakstuk.Name.Substring(geselecteerdschaakstuk.Name.Length - 1);
                    geselecteerdschaakstuk.BackgroundImage = ZwarteDame.BackgroundImage;
                }
                beurtVeranderen();
            }
        }
        private void eerstezet()
        {
            //eerste zet gebruiken
            if (geselecteerdschaakstuk.Name.Substring(0, geselecteerdschaakstuk.Name.Length - 1) == "WittePion")
            {
                PionEersteZetWit[Convert.ToInt32(geselecteerdschaakstuk.Name.Substring(geselecteerdschaakstuk.Name.Length - 1)) - 1] = false;
            }
            else if (geselecteerdschaakstuk.Name.Substring(0, geselecteerdschaakstuk.Name.Length - 1) == "ZwartePion")
            {
                PionEersteZetZwart[Convert.ToInt32(geselecteerdschaakstuk.Name.Substring(geselecteerdschaakstuk.Name.Length - 1)) - 1] = false;
            }
            else if (geselecteerdschaakstuk.Name.Substring(0, geselecteerdschaakstuk.Name.Length - 1) == "WitteToren")
            {
                TorenEersteZetWit[Convert.ToInt32(geselecteerdschaakstuk.Name.Substring(geselecteerdschaakstuk.Name.Length - 1)) - 1] = false;
            }
            else if (geselecteerdschaakstuk.Name.Substring(0, geselecteerdschaakstuk.Name.Length - 1) == "ZwarteToren")
            {
                TorenEersteZetZwart[Convert.ToInt32(geselecteerdschaakstuk.Name.Substring(geselecteerdschaakstuk.Name.Length - 1)) - 1] = false;
            }
            else if (geselecteerdschaakstuk.Name == "WitteKoning")
            {
                KoningEersteZetWit = false;
            }
            else if (geselecteerdschaakstuk.Name == "ZwarteKoning")
            {
                KoningEersteZetZwart = false;
            }
        }
        private void achtergrondtransparent()
        {
            //achtergrond transparent maken en beurt veranderen
            foreach (var item in schaakstukken)
            {
                item.BackColor = Color.Transparent;
            }
        }
        private void zettenontzichtbaar()
        {
            //zetten ontzichtbaar maken
            foreach (var item in zetten)
            {
                item.Visible = false;
            }
        }
        private void SchaakstukNemen(PictureBox geselecteerdschaakstuk, PictureBox genomenstuk)
        {
            achtergrondtransparent();
            zettenontzichtbaar();
            eerstezet();
            string kleur;
            PictureBox koning;
            schaak = false;
            if (beurt == 1)
            {
                kleur = "Witte";
                koning = ZwarteKoning;
            }
            else
            {
                kleur = "Zwarte";
                koning = WitteKoning;
            }
            if (!geselecteerdschaakstuk.Name.Contains("Koning"))
            {
                #region toren en dame nakijken
                for (int i = koning.Location.Y - 75; i >= 0; i = i - 75)
                {
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == i && element.Location.X == koning.Location.X);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk && PicSchaakstuk != genomenstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Toren") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde1;
                    }
                    else if (genomenstuk.Location.Y == i && genomenstuk.Location.X == koning.Location.X)
                    {
                        goto einde1;
                    }
                }
            einde1:
                for (int i = koning.Location.Y + 75; i <= 600; i = i + 75)
                {
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == i && element.Location.X == koning.Location.X);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk && PicSchaakstuk != genomenstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Toren") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde2;
                    }
                    else if (genomenstuk.Location.Y == i && genomenstuk.Location.X == koning.Location.X)
                    {
                        goto einde2;
                    }
                }
            einde2:
                for (int i = koning.Location.X + 75; i <= 600; i = i + 75)
                {
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.X == i && element.Location.Y == koning.Location.Y);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk && PicSchaakstuk != genomenstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Toren") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde3;
                    }
                    else if (genomenstuk.Location.X == i && genomenstuk.Location.Y == koning.Location.Y)
                    {
                        goto einde3;
                    }
                }
            einde3:
                for (int i = koning.Location.X - 75; i >= 0; i = i - 75)
                {
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.X == i && element.Location.Y == koning.Location.Y);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk && PicSchaakstuk != genomenstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Toren") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde4;
                    }
                    else if (genomenstuk.Location.X == i && genomenstuk.Location.Y == koning.Location.Y)
                    {
                        goto einde4;
                    }
                }
            einde4:;
                #endregion
                #region loper en dame nakijken
                int j = koning.Location.X;
                for (int i = koning.Location.Y - 75; i >= 0; i = i - 75)
                {
                    j = j - 75;
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == i && element.Location.X == j);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk && PicSchaakstuk != genomenstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Loper") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde5;
                    }
                    else if (genomenstuk.Location.Y == i && genomenstuk.Location.X == j)
                    {
                        goto einde5;
                    }

                }
            einde5:;
                j = koning.Location.X;
                for (int i = koning.Location.Y + 75; i <= 600; i = i + 75)
                {
                    j = j - 75;
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == i && element.Location.X == j);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk && PicSchaakstuk != genomenstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Loper") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde6;
                    }
                    else if (genomenstuk.Location.Y == i && genomenstuk.Location.X == j)
                    {
                        goto einde6;
                    }
                }
            einde6:;
                j = koning.Location.Y;
                for (int i = koning.Location.X + 75; i <= 600; i = i + 75)
                {
                    j = j - 75;
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.X == i && element.Location.Y == j);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk && PicSchaakstuk != genomenstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Loper") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde7;
                    }
                    else if (genomenstuk.Location.X == i && genomenstuk.Location.Y == j)
                    {
                        goto einde7;
                    }
                }
            einde7:;
                j = koning.Location.Y;
                for (int i = koning.Location.X + 75; i <= 600; i = i + 75)
                {
                    j = j + 75;
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.X == i && element.Location.Y == j);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk && PicSchaakstuk != genomenstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Loper") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde8;
                    }
                    else if (genomenstuk.Location.X == i && genomenstuk.Location.Y == j)
                    {
                        goto einde8;
                    }
                }
            einde8:;
                #endregion
                #region paard nakijken
                foreach (var item in schaakstukken)
                {
                    if (koning.Location.X + 75 == item.Location.X && koning.Location.Y - 150 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                if (item !=genomenstuk)
                                {
                                    item.BackColor = Color.FromArgb(0, 0, 255);
                                    schaak = true;
                                }
                            }
                        }
                    }
                    if (koning.Location.X - 75 == item.Location.X && koning.Location.Y - 150 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                if (item != genomenstuk)
                                {
                                    item.BackColor = Color.FromArgb(0, 0, 255);
                                    schaak = true;
                                }
                            }
                        }
                    }
                    if (koning.Location.X + 75 == item.Location.X && koning.Location.Y + 150 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                if (item != genomenstuk)
                                {
                                    item.BackColor = Color.FromArgb(0, 0, 255);
                                    schaak = true;
                                }
                            }
                        }
                    }
                    if (koning.Location.X - 75 == item.Location.X && koning.Location.Y + 150 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                if (item != genomenstuk)
                                {
                                    item.BackColor = Color.FromArgb(0, 0, 255);
                                    schaak = true;
                                }
                            }
                        }
                    }
                    if (koning.Location.X + 150 == item.Location.X && koning.Location.Y - 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                if (item != genomenstuk)
                                {
                                    item.BackColor = Color.FromArgb(0, 0, 255);
                                    schaak = true;
                                }
                            }
                        }
                    }
                    if (koning.Location.X - 150 == item.Location.X && koning.Location.Y - 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                if (item != genomenstuk)
                                {
                                    item.BackColor = Color.FromArgb(0, 0, 255);
                                    schaak = true;
                                }
                            }
                        }
                    }
                    if (koning.Location.X + 150 == item.Location.X && koning.Location.Y + 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                if (item != genomenstuk)
                                {
                                    item.BackColor = Color.FromArgb(0, 0, 255);
                                    schaak = true;
                                }
                            }
                        }
                    }
                    if (koning.Location.X - 150 == item.Location.X && koning.Location.Y + 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                if (item != genomenstuk)
                                {
                                    item.BackColor = Color.FromArgb(0, 0, 255);
                                    schaak = true;
                                }
                            }
                        }
                    }
                }
                #endregion
                #region koning nakijken
                foreach (var item in schaakstukken)
                {
                    if (koning.Location.X + 75 == item.Location.X && koning.Location.Y == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X - 75 == item.Location.X && koning.Location.Y == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X + 75 == item.Location.X && koning.Location.Y + 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X - 75 == item.Location.X && koning.Location.Y + 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X == item.Location.X && koning.Location.Y - 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X == item.Location.X && koning.Location.Y + 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X - 75 == item.Location.X && koning.Location.Y - 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X + 75 == item.Location.X && koning.Location.Y - 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                }
                #endregion
                #region pion nakijken
                if (kleur == "Zwarte")
                {
                    foreach (var item in schaakstukken)
                    {
                        if (koning.Location.X == item.Location.X - 75 && koning.Location.Y == item.Location.Y + 75)
                        {
                            if (item.Name.Contains(kleur))
                            {
                                if (item.Name.Contains("Pion"))
                                {
                                    if (item != genomenstuk)
                                    {
                                        item.BackColor = Color.FromArgb(0, 0, 255);
                                        schaak = true;
                                    }
                                }
                            }
                        }
                        if (koning.Location.X == item.Location.X + 75 && koning.Location.Y == item.Location.Y + 75)
                        {
                            if (item.Name.Contains(kleur))
                            {
                                if (item.Name.Contains("Pion"))
                                {
                                    if (item != genomenstuk)
                                    {
                                        item.BackColor = Color.FromArgb(0, 0, 255);
                                        schaak = true;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in schaakstukken)
                    {
                        if (koning.Location.X == item.Location.X - 75 && koning.Location.Y == item.Location.Y - 75)
                        {
                            if (item.Name.Contains(kleur))
                            {
                                if (item.Name.Contains("Pion"))
                                {
                                    if (item != genomenstuk)
                                    {
                                        item.BackColor = Color.FromArgb(0, 0, 255);
                                        schaak = true;
                                    }
                                }
                            }
                        }
                        if (koning.Location.X == item.Location.X + 75 && koning.Location.Y == item.Location.Y - 75)
                        {
                            if (item.Name.Contains(kleur))
                            {
                                if (item.Name.Contains("Pion"))
                                {
                                    if (item != genomenstuk)
                                    {
                                        item.BackColor = Color.FromArgb(0, 0, 255);
                                        schaak = true;
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
                //locatie schaakstuk aanpassen en genomen stuk ontzichtbaar maken
                if (schaak==false)
                {
                    genomenstuk.Visible = false;
                    geselecteerdschaakstuk.Location = new Point(genomenstuk.Location.X, genomenstuk.Location.Y);
                    genomenstuk.Location = new Point(-100, -100);
                    //beurten veranderen
                    beurtVeranderen();
                }
            }
            else
            {
                #region toren en dame nakijken
                for (int i = genomenstuk.Location.Y - 75; i >= 0; i = i - 75)
                {
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == i && element.Location.X == genomenstuk.Location.X);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk && PicSchaakstuk != genomenstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Toren") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde1;
                    }
                }
            einde1:
                for (int i = genomenstuk.Location.Y + 75; i <= 600; i = i + 75)
                {
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == i && element.Location.X == genomenstuk.Location.X);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk && PicSchaakstuk != genomenstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Toren") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde2;
                    }
                }
            einde2:
                for (int i = genomenstuk.Location.X + 75; i <= 600; i = i + 75)
                {
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.X == i && element.Location.Y == genomenstuk.Location.Y);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk && PicSchaakstuk != genomenstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Toren") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde3;
                    }
                }
            einde3:
                for (int i = genomenstuk.Location.X - 75; i >= 0; i = i - 75)
                {
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.X == i && element.Location.Y == genomenstuk.Location.Y);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk && PicSchaakstuk != genomenstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Toren") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde4;
                    }
                }
            einde4:;
                #endregion
                #region loper en dame nakijken
                int j = genomenstuk.Location.X;
                for (int i = genomenstuk.Location.Y - 75; i >= 0; i = i - 75)
                {
                    j = j - 75;
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == i && element.Location.X == j);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk && PicSchaakstuk != genomenstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Loper") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde5;
                    }

                }
            einde5:;
                j = genomenstuk.Location.X;
                for (int i = genomenstuk.Location.Y + 75; i <= 600; i = i + 75)
                {
                    j = j - 75;
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == i && element.Location.X == j);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk && PicSchaakstuk != genomenstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Loper") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde6;
                    }
                }
            einde6:;
                j = genomenstuk.Location.Y;
                for (int i = genomenstuk.Location.X + 75; i <= 600; i = i + 75)
                {
                    j = j - 75;
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.X == i && element.Location.Y == j);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk && PicSchaakstuk != genomenstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Loper") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde7;
                    }
                }
            einde7:;
                j = genomenstuk.Location.Y;
                for (int i = genomenstuk.Location.X + 75; i <= 600; i = i + 75)
                {
                    j = j + 75;
                    PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.X == i && element.Location.Y == j);
                    if (PicSchaakstuk != null && PicSchaakstuk != geselecteerdschaakstuk && PicSchaakstuk != genomenstuk)
                    {
                        if (PicSchaakstuk.Name.Contains(kleur))
                        {
                            if (PicSchaakstuk.Name.Contains("Loper") || PicSchaakstuk.Name.Contains("Dame"))
                            {
                                PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                        goto einde8;
                    }
                }
            einde8:;
                #endregion
                #region paard nakijken
                foreach (var item in schaakstukken)
                {
                    if (genomenstuk.Location.X + 75 == item.Location.X && genomenstuk.Location.Y - 150 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                if (item != genomenstuk)
                                {
                                    item.BackColor = Color.FromArgb(0, 0, 255);
                                    schaak = true;
                                }
                            }
                        }
                    }
                    if (genomenstuk.Location.X - 75 == item.Location.X && genomenstuk.Location.Y - 150 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                if (item != genomenstuk)
                                {
                                    item.BackColor = Color.FromArgb(0, 0, 255);
                                    schaak = true;
                                }
                            }
                        }
                    }
                    if (genomenstuk.Location.X + 75 == item.Location.X && genomenstuk.Location.Y + 150 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                if (item != genomenstuk)
                                {
                                    item.BackColor = Color.FromArgb(0, 0, 255);
                                    schaak = true;
                                }
                            }
                        }
                    }
                    if (genomenstuk.Location.X - 75 == item.Location.X && genomenstuk.Location.Y + 150 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                if (item != genomenstuk)
                                {
                                    item.BackColor = Color.FromArgb(0, 0, 255);
                                    schaak = true;
                                }
                            }
                        }
                    }
                    if (genomenstuk.Location.X + 150 == item.Location.X && genomenstuk.Location.Y - 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                if (item != genomenstuk)
                                {
                                    item.BackColor = Color.FromArgb(0, 0, 255);
                                    schaak = true;
                                }
                            }
                        }
                    }
                    if (genomenstuk.Location.X - 150 == item.Location.X && genomenstuk.Location.Y - 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                if (item != genomenstuk)
                                {
                                    item.BackColor = Color.FromArgb(0, 0, 255);
                                    schaak = true;
                                }
                            }
                        }
                    }
                    if (genomenstuk.Location.X + 150 == item.Location.X && genomenstuk.Location.Y + 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                if (item != genomenstuk)
                                {
                                    item.BackColor = Color.FromArgb(0, 0, 255);
                                    schaak = true;
                                }
                            }
                        }
                    }
                    if (genomenstuk.Location.X - 150 == item.Location.X && genomenstuk.Location.Y + 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Paard"))
                            {
                                if (item != genomenstuk)
                                {
                                    item.BackColor = Color.FromArgb(0, 0, 255);
                                    schaak = true;
                                }
                            }
                        }
                    }
                }
                #endregion
                #region koning nakijken
                foreach (var item in schaakstukken)
                {
                    if (genomenstuk.Location.X + 75 == item.Location.X && genomenstuk.Location.Y == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (genomenstuk.Location.X - 75 == item.Location.X && genomenstuk.Location.Y == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (genomenstuk.Location.X + 75 == item.Location.X && genomenstuk.Location.Y + 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (genomenstuk.Location.X - 75 == item.Location.X && genomenstuk.Location.Y + 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (genomenstuk.Location.X == item.Location.X && genomenstuk.Location.Y - 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (genomenstuk.Location.X == item.Location.X && genomenstuk.Location.Y + 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (genomenstuk.Location.X - 75 == item.Location.X && genomenstuk.Location.Y - 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (genomenstuk.Location.X + 75 == item.Location.X && genomenstuk.Location.Y - 75 == item.Location.Y)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Koning"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                }
                #endregion
                #region pion nakijken
                if (kleur == "Zwarte")
                {
                    foreach (var item in schaakstukken)
                    {
                        if (genomenstuk.Location.X == item.Location.X - 75 && genomenstuk.Location.Y == item.Location.Y + 75)
                        {
                            if (item.Name.Contains(kleur))
                            {
                                if (item.Name.Contains("Pion"))
                                {
                                    if (item != genomenstuk)
                                    {
                                        item.BackColor = Color.FromArgb(0, 0, 255);
                                        schaak = true;
                                    }
                                }
                            }
                        }
                        if (genomenstuk.Location.X == item.Location.X + 75 && genomenstuk.Location.Y == item.Location.Y + 75)
                        {
                            if (item.Name.Contains(kleur))
                            {
                                if (item.Name.Contains("Pion"))
                                {
                                    if (item != genomenstuk)
                                    {
                                        item.BackColor = Color.FromArgb(0, 0, 255);
                                        schaak = true;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in schaakstukken)
                    {
                        if (genomenstuk.Location.X == item.Location.X - 75 && genomenstuk.Location.Y == item.Location.Y - 75)
                        {
                            if (item.Name.Contains(kleur))
                            {
                                if (item.Name.Contains("Pion"))
                                {
                                    if (item != genomenstuk)
                                    {
                                        item.BackColor = Color.FromArgb(0, 0, 255);
                                        schaak = true;
                                    }
                                }
                            }
                        }
                        if (genomenstuk.Location.X == item.Location.X + 75 && genomenstuk.Location.Y == item.Location.Y - 75)
                        {
                            if (item.Name.Contains(kleur))
                            {
                                if (item.Name.Contains("Pion"))
                                {
                                    if (item != genomenstuk)
                                    {
                                        item.BackColor = Color.FromArgb(0, 0, 255);
                                        schaak = true;
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
                //locatie schaakstuk aanpassen en genomen stuk ontzichtbaar maken
                if (schaak == false)
                {
                    genomenstuk.Visible = false;
                    geselecteerdschaakstuk.Location = new Point(genomenstuk.Location.X, genomenstuk.Location.Y);
                    genomenstuk.Location = new Point(-100, -100);
                    //beurten veranderen
                    beurtVeranderen();
                }
            }


            //Witte pion veranderen in witte dame wanneer hij aan de overkant geraakt
            if (geselecteerdschaakstuk.Name.Substring(0, geselecteerdschaakstuk.Name.Length - 1) == "WittePion" && geselecteerdschaakstuk.Location.Y == 0)
            {
                geselecteerdschaakstuk.Name = "WitteDame" + geselecteerdschaakstuk.Name.Substring(geselecteerdschaakstuk.Name.Length - 1);
                geselecteerdschaakstuk.BackgroundImage = WitteDame.BackgroundImage;
            }
            //Zwarte pion veranderen in Zwarte dame wanneer hij aan de overkant geraakt
            if (geselecteerdschaakstuk.Name.Substring(0, geselecteerdschaakstuk.Name.Length - 1) == "ZwartePion" && geselecteerdschaakstuk.Location.Y == 525)
            {
                geselecteerdschaakstuk.Name = "ZwarteDame" + geselecteerdschaakstuk.Name.Substring(geselecteerdschaakstuk.Name.Length - 1);
                geselecteerdschaakstuk.BackgroundImage = ZwarteDame.BackgroundImage;
            }
            //wanneer de koning genomen wordt een bericht laten zien
            if (genomenstuk==ZwarteKoning)
            {
                MessageBox.Show("Wit wint", "Winnaar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
            if (genomenstuk == WitteKoning)
            {
                MessageBox.Show("Zwart wint", "Winnaar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
        }
        private void beurtVeranderen()
        {
            //als de beurt 1 is dan wordt het 0 of andersom
            if (beurt == 1)
            {
                beurt = 0;
                SchaakNakijken(WitteKoning, "Zwarte");
            }
            else
            {
                beurt = 1;
                SchaakNakijken(ZwarteKoning, "Witte");
            }
        }
        private void SchaakNakijken(PictureBox koning, string kleur)
        {
            schaak = false;
            #region toren en dame nakijken
            for (int i = koning.Location.Y - 75; i >= 0; i = i - 75)
            {
                PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == i && element.Location.X == koning.Location.X);
                if (PicSchaakstuk != null)
                {
                    if (PicSchaakstuk.Name.Contains(kleur))
                    {
                        if (PicSchaakstuk.Name.Contains("Toren") || PicSchaakstuk.Name.Contains("Dame"))
                        {
                            PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                            schaak = true;
                        }
                    }
                    goto einde1;
                }
            }
        einde1:
            for (int i = koning.Location.Y + 75; i <= 600; i = i + 75)
            {
                PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == i && element.Location.X == koning.Location.X);
                if (PicSchaakstuk != null)
                {
                    if (PicSchaakstuk.Name.Contains(kleur))
                    {
                        if (PicSchaakstuk.Name.Contains("Toren") || PicSchaakstuk.Name.Contains("Dame"))
                        {
                            PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                            schaak = true;
                        }
                    }
                    goto einde2;
                }
            }
        einde2:
            for (int i = koning.Location.X + 75; i <= 600; i = i + 75)
            {
                PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.X == i && element.Location.Y == koning.Location.Y);
                if (PicSchaakstuk != null)
                {
                    if (PicSchaakstuk.Name.Contains(kleur))
                    {
                        if (PicSchaakstuk.Name.Contains("Toren") || PicSchaakstuk.Name.Contains("Dame"))
                        {
                            PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                            schaak = true;
                        }
                    }
                    goto einde3;
                }
            }
        einde3:
            for (int i = koning.Location.X - 75; i >= 0; i = i - 75)
            {
                PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.X == i && element.Location.Y == koning.Location.Y);
                if (PicSchaakstuk != null)
                {
                    if (PicSchaakstuk.Name.Contains(kleur))
                    {
                        if (PicSchaakstuk.Name.Contains("Toren") || PicSchaakstuk.Name.Contains("Dame"))
                        {
                            PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                            schaak = true;
                        }
                    }
                    goto einde4;
                }
            }
        einde4:;
            #endregion
            #region loper en dame nakijken
            int j = koning.Location.X;
            for (int i = koning.Location.Y - 75; i >= 0; i = i - 75)
            {
                j = j - 75;
                PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == i && element.Location.X == j);
                if (PicSchaakstuk != null)
                {
                    if (PicSchaakstuk.Name.Contains(kleur))
                    {
                        if (PicSchaakstuk.Name.Contains("Loper") || PicSchaakstuk.Name.Contains("Dame"))
                        {
                            PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                            schaak = true;
                        }
                    }
                    goto einde5;
                }

            }
        einde5:;
            j = koning.Location.X;
            for (int i = koning.Location.Y + 75; i <= 600; i = i + 75)
            {
                j = j - 75;
                PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.Y == i && element.Location.X == j);
                if (PicSchaakstuk != null)
                {
                    if (PicSchaakstuk.Name.Contains(kleur))
                    {
                        if (PicSchaakstuk.Name.Contains("Loper") || PicSchaakstuk.Name.Contains("Dame"))
                        {
                            PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                            schaak = true;
                        }
                    }
                    goto einde6;
                }
            }
        einde6:;
            j = koning.Location.Y;
            for (int i = koning.Location.X + 75; i <= 600; i = i + 75)
            {
                j = j - 75;
                PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.X == i && element.Location.Y == j);
                if (PicSchaakstuk != null)
                {
                    if (PicSchaakstuk.Name.Contains(kleur))
                    {
                        if (PicSchaakstuk.Name.Contains("Loper") || PicSchaakstuk.Name.Contains("Dame"))
                        {
                            PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                            schaak = true;
                        }
                    }
                    goto einde7;
                }
            }
        einde7:;
            j = koning.Location.Y;
            for (int i = koning.Location.X + 75; i <= 600; i = i + 75)
            {
                j = j + 75;
                PicSchaakstuk = Array.Find(schaakstukken, element => element.Location.X == i && element.Location.Y == j);
                if (PicSchaakstuk != null)
                {
                    if (PicSchaakstuk.Name.Contains(kleur))
                    {
                        if (PicSchaakstuk.Name.Contains("Loper") || PicSchaakstuk.Name.Contains("Dame"))
                        {
                            PicSchaakstuk.BackColor = Color.FromArgb(0, 0, 255);
                            schaak = true;
                        }
                    }
                    goto einde8;
                }
            }
        einde8:;
            #endregion
            #region paard nakijken
            foreach (var item in schaakstukken)
            {
                if (koning.Location.X + 75 == item.Location.X && koning.Location.Y - 150 == item.Location.Y)
                {
                    if (item.Name.Contains(kleur))
                    {
                        if (item.Name.Contains("Paard"))
                        {
                            item.BackColor = Color.FromArgb(0, 0, 255);
                            schaak = true;
                        }
                    }
                }
                if (koning.Location.X - 75 == item.Location.X && koning.Location.Y - 150 == item.Location.Y)
                {
                    if (item.Name.Contains(kleur))
                    {
                        if (item.Name.Contains("Paard"))
                        {
                            item.BackColor = Color.FromArgb(0, 0, 255);
                            schaak = true;
                        }
                    }
                }
                if (koning.Location.X + 75 == item.Location.X && koning.Location.Y + 150 == item.Location.Y)
                {
                    if (item.Name.Contains(kleur))
                    {
                        if (item.Name.Contains("Paard"))
                        {
                            item.BackColor = Color.FromArgb(0, 0, 255);
                            schaak = true;
                        }
                    }
                }
                if (koning.Location.X - 75 == item.Location.X && koning.Location.Y + 150 == item.Location.Y)
                {
                    if (item.Name.Contains(kleur))
                    {
                        if (item.Name.Contains("Paard"))
                        {
                            item.BackColor = Color.FromArgb(0, 0, 255);
                            schaak = true;
                        }
                    }
                }
                if (koning.Location.X + 150 == item.Location.X && koning.Location.Y - 75 == item.Location.Y)
                {
                    if (item.Name.Contains(kleur))
                    {
                        if (item.Name.Contains("Paard"))
                        {
                            item.BackColor = Color.FromArgb(0, 0, 255);
                            schaak = true;
                        }
                    }
                }
                if (koning.Location.X - 150 == item.Location.X && koning.Location.Y - 75 == item.Location.Y)
                {
                    if (item.Name.Contains(kleur))
                    {
                        if (item.Name.Contains("Paard"))
                        {
                            item.BackColor = Color.FromArgb(0, 0, 255);
                            schaak = true;
                        }
                    }
                }
                if (koning.Location.X + 150 == item.Location.X && koning.Location.Y + 75 == item.Location.Y)
                {
                    if (item.Name.Contains(kleur))
                    {
                        if (item.Name.Contains("Paard"))
                        {
                            item.BackColor = Color.FromArgb(0, 0, 255);
                            schaak = true;
                        }
                    }
                }
                if (koning.Location.X - 150 == item.Location.X && koning.Location.Y + 75 == item.Location.Y)
                {
                    if (item.Name.Contains(kleur))
                    {
                        if (item.Name.Contains("Paard"))
                        {
                            item.BackColor = Color.FromArgb(0, 0, 255);
                            schaak = true;
                        }
                    }
                }
            }
            #endregion
            #region koning nakijken
            foreach (var item in schaakstukken)
            {
                if (koning.Location.X + 75 == item.Location.X && koning.Location.Y == item.Location.Y)
                {
                    if (item.Name.Contains(kleur))
                    {
                        if (item.Name.Contains("Koning"))
                        {
                            item.BackColor = Color.FromArgb(0, 0, 255);
                            schaak = true;
                        }
                    }
                }
                if (koning.Location.X - 75 == item.Location.X && koning.Location.Y == item.Location.Y)
                {
                    if (item.Name.Contains(kleur))
                    {
                        if (item.Name.Contains("Koning"))
                        {
                            item.BackColor = Color.FromArgb(0, 0, 255);
                            schaak = true;
                        }
                    }
                }
                if (koning.Location.X + 75 == item.Location.X && koning.Location.Y + 75 == item.Location.Y)
                {
                    if (item.Name.Contains(kleur))
                    {
                        if (item.Name.Contains("Koning"))
                        {
                            item.BackColor = Color.FromArgb(0, 0, 255);
                            schaak = true;
                        }
                    }
                }
                if (koning.Location.X - 75 == item.Location.X && koning.Location.Y + 75 == item.Location.Y)
                {
                    if (item.Name.Contains(kleur))
                    {
                        if (item.Name.Contains("Koning"))
                        {
                            item.BackColor = Color.FromArgb(0, 0, 255);
                            schaak = true;
                        }
                    }
                }
                if (koning.Location.X == item.Location.X && koning.Location.Y - 75 == item.Location.Y)
                {
                    if (item.Name.Contains(kleur))
                    {
                        if (item.Name.Contains("Koning"))
                        {
                            item.BackColor = Color.FromArgb(0, 0, 255);
                            schaak = true;
                        }
                    }
                }
                if (koning.Location.X == item.Location.X && koning.Location.Y + 75 == item.Location.Y)
                {
                    if (item.Name.Contains(kleur))
                    {
                        if (item.Name.Contains("Koning"))
                        {
                            item.BackColor = Color.FromArgb(0, 0, 255);
                            schaak = true;
                        }
                    }
                }
                if (koning.Location.X - 75 == item.Location.X && koning.Location.Y - 75 == item.Location.Y)
                {
                    if (item.Name.Contains(kleur))
                    {
                        if (item.Name.Contains("Koning"))
                        {
                            item.BackColor = Color.FromArgb(0, 0, 255);
                            schaak = true;
                        }
                    }
                }
                if (koning.Location.X + 75 == item.Location.X && koning.Location.Y - 75 == item.Location.Y)
                {
                    if (item.Name.Contains(kleur))
                    {
                        if (item.Name.Contains("Koning"))
                        {
                            item.BackColor = Color.FromArgb(0, 0, 255);
                            schaak = true;
                        }
                    }
                }
            }
            #endregion
            #region pion nakijken
            if (kleur=="Zwarte")
            {
                foreach (var item in schaakstukken)
                {
                    if (koning.Location.X == item.Location.X - 75 && koning.Location.Y == item.Location.Y + 75)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Pion"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X == item.Location.X + 75 && koning.Location.Y == item.Location.Y + 75)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Pion"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (var item in schaakstukken)
                {
                    if (koning.Location.X == item.Location.X - 75 && koning.Location.Y == item.Location.Y - 75)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Pion"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                    if (koning.Location.X == item.Location.X + 75 && koning.Location.Y == item.Location.Y - 75)
                    {
                        if (item.Name.Contains(kleur))
                        {
                            if (item.Name.Contains("Pion"))
                            {
                                item.BackColor = Color.FromArgb(0, 0, 255);
                                schaak = true;
                            }
                        }
                    }
                }
            }
            #endregion
            if (schaak == true)
            {
                MessageBox.Show("U staat schaak", "Schaak", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion
        #endregion
        #region timer
        //timer code
        private void timer1_Tick(object sender, EventArgs e)
        {
            //beurt nakijken
            if (beurt == 1)
            {
                ZwartTimerMinuten = Convert.ToInt32(TimerZwart.Text.Substring(0,2));
                ZwartTimerSeconden = Convert.ToInt32(TimerZwart.Text.Substring(4, 3).Replace(":", "  "));
                if (ZwartTimerSeconden==0) //tijd aanpassen
                {
                    ZwartTimerMinuten--;
                    ZwartTimerSeconden = 59;
                }
                else
                {
                    ZwartTimerSeconden--;
                }
                TimerZwart.Text = ZwartTimerMinuten + " : " + ZwartTimerSeconden+"  ";
                if (ZwartTimerMinuten == 0 && ZwartTimerSeconden == 0) //indien tijd 0 is winnaar weergeven
                {
                    timer1.Enabled=false;
                    MessageBox.Show("Wit wint", "Winnaar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Exit();
                }
            }
            else
            {
                WitTimerMinuten = Convert.ToInt32(TimerWit.Text.Substring(0, 2));
                WitTimerSeconden = Convert.ToInt32(TimerWit.Text.Substring(4, 3).Replace(":","  "));
                if (WitTimerSeconden == 0) //tijd aanpassen
                {
                    WitTimerMinuten--;
                    WitTimerSeconden = 59;
                }
                else
                {
                    WitTimerSeconden--;
                }
                TimerWit.Text = WitTimerMinuten + " : " + WitTimerSeconden+"  ";
                if (WitTimerMinuten == 0 && WitTimerSeconden == 0) //indien tijd 0 is winnaar weergeven
                {
                    timer1.Enabled = false;
                    MessageBox.Show("Zwart wint", "Winnaar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Exit();
                }
            }
        }
        #endregion
    }
}