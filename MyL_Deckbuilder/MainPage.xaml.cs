using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Xml;
using StringExtensions;

namespace MyL_Deckbuilder
{
    public partial class MainPage : UserControl
    {
        private DeckStats currentDeck = new DeckStats();

        public MainPage()
        {
            InitializeComponent();
            LoadCardCollection("");

            System.Windows.Browser.HtmlPage.Plugin.Focus();
        }

        private void LoadCardCollection(string qry)
        {
            XDocument xmlCards = XDocument.Load(@"MyL_Furia.xml");
            var qrs = qry.Minimize().Trim().Split();

            var cards = from card in xmlCards.Descendants("Carta")
                        where card.Element("nombre").ToString().Minimize().ContainsAll(qrs)
                        || card.Element("habilidad").ToString().Minimize().ContainsAll(qrs)
                        select new Carta
                        {
                            IdCarta = (string)card.Element("idcarta"),
                            Nombre = (string)card.Element("nombre"),
                            Frecuencia = (string)card.Element("frecuencia") == "null" ? "-" : (string)card.Element("frecuencia"),
                            Tipo = (string)card.Element("tipo"),
                            Coste = (int)card.Element("coste") >= 0 ? (string)card.Element("coste") : "-",
                            Fuerza = (int)card.Element("fuerza") >= 0 ? (string)card.Element("fuerza") : "-",
                            Raza = (string)card.Element("raza") == "null" ? "-" : (string)card.Element("raza"),
                            esUnica = (bool)card.Element("esunica"),
                            Habilidad = (string)card.Element("habilidad"),
                            ImgSrc = "img/" + (string)card.Element("idcarta") + ".jpg"
                        };
            CardList.ItemsSource = cards;

            var deckCards = new List<DeckCard>();
            DeckList.ItemsSource = deckCards;
            CardDetailView.Visibility = Visibility.Collapsed;
        }

        private void Button_QueryCollection(object sender, RoutedEventArgs e)
        {
            LoadCardCollection(QryTxtBox.Text);
        }

        private void TextBox_SelectAll(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void TextBox_EnterKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_QueryCollection(sender, null);
            }
        }

        private void Button_AddCardToDeck(object sender, RoutedEventArgs e)
        {
            Carta carta = (Carta)((Button)sender).DataContext;
            AddCardToDeck(carta);            
        }

        private void Button_RemoveCardFromDeck(object sender, RoutedEventArgs e)
        {
            Carta carta = (Carta)((Button)sender).DataContext;
            RemoveCardFromDeck(carta);
        }

        private void Button_AddCardToDeck_2(object sender, RoutedEventArgs e)
        {
            DeckCard deckCard = (DeckCard)((Button)sender).DataContext;
            var cardList = currentDeck.CardsInDeck;
            Carta carta = cardList.First(c => c.Nombre == deckCard.Nombre);
            AddCardToDeck(carta);
        }

        private void Button_RemoveCardFromDeck_2(object sender, RoutedEventArgs e)
        {
            DeckCard deckCard = (DeckCard)((Button)sender).DataContext;
            var cardList = currentDeck.CardsInDeck;
            Carta carta = cardList.First(c => c.Nombre == deckCard.Nombre);
            RemoveCardFromDeck(carta);
        }

        private void AddCardToDeck(Carta carta)
        {
            var deckList = (List<DeckCard>)DeckList.ItemsSource;

            if (currentDeck.CardsInDeck.Contains(carta))
            {
                if (carta.Tipo == "Oro")
                {
                    deckList.First(c => c.Nombre == carta.Nombre).Cantidad += 1;
                    ChangeCardStats(carta, 1);
                    UpdateDeckList(deckList);
                    return;
                }
                if (!carta.esUnica)
                {
                    int cant = deckList.First(c => c.Nombre == carta.Nombre).Cantidad;
                    deckList.First(c => c.Nombre == carta.Nombre).Cantidad += cant < 3 ? 1 : 0;
                    if (deckList.First(c => c.Nombre == carta.Nombre).Cantidad != cant)
                    {
                        ChangeCardStats(carta, 1);
                        UpdateDeckList(deckList);
                    }
                }
            }
            else
            {
                deckList.Add(new DeckCard { Cantidad = 1, IdCarta = carta.IdCarta, Nombre = carta.Nombre });
                currentDeck.CardsInDeck.Add(carta);

                ChangeCardStats(carta, 1);
                UpdateDeckList(deckList);
            }
        }

        private void RemoveCardFromDeck(Carta carta)
        {
            var deckList = (List<DeckCard>)DeckList.ItemsSource;

            if (currentDeck.CardsInDeck.Contains(carta))
            {
                int cant = deckList.First(c => c.Nombre == carta.Nombre).Cantidad;
                if (cant > 1)
                {
                    deckList.First(c => c.Nombre == carta.Nombre).Cantidad -= 1;
                }
                else
                {
                    deckList.RemoveAll(c => c.Nombre == carta.Nombre);
                    currentDeck.CardsInDeck.Remove(carta);
                }

                ChangeCardStats(carta, -1);
                UpdateDeckList(deckList);
            }
        }

        private void ChangeCardStats(Carta carta, int mod)
        {
            currentDeck.DeckCount += mod;

            int coste;
            try
            {
                
            coste = int.Parse(carta.Coste);
            }
            catch (Exception)
            {
                
                coste = -1;
            }

            if (coste >= 0)
            {
                coste = coste > 6 ? 7 : coste;
                currentDeck.GoldCosts[coste] += mod;
            }
            switch (carta.Tipo)
            {
                case "Aliado":
                    currentDeck.AllyCount += mod;
                    break;
                case "Talismán":
                    currentDeck.TalismanCount += mod;
                    break;
                case "Arma":
                    currentDeck.WeaponCount += mod;
                    break;
                case "Tótem":
                    currentDeck.TotemCount += mod;
                    break;
                default:
                    currentDeck.GoldCount += mod;
                    break;
            }
        }

        private void UpdateDeckList(List<DeckCard> deckList)
        {
            deckList.Sort((x, y) => string.Compare(x.IdCarta, y.IdCarta));
            DeckList.ItemsSource = null;
            DeckList.ItemsSource = deckList;
            TotalTxtBox.Text = "Total: " + currentDeck.DeckCount;
        }

        private void CardList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Carta carta = (Carta)CardList.SelectedItem;

            CardDetailView.DataContext = carta;
            CardDetailView.Visibility = Visibility.Visible;
        }

        private void Button_ShowStats(object sender, RoutedEventArgs e)
        {
            currentDeck.UpdateCharts();
            StatsPageView.DataContext = null;
            StatsPageView.DataContext = currentDeck;
            StatsPageView.Visibility = Visibility.Visible;
        }
        
    }
}
