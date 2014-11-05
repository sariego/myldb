using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace MyL_Deckbuilder
{
    public class DeckStats
    {
        public DeckStats()
        {
            this.CardsInDeck = new List<Carta>();
            this.DeckCount = 0;
            this.AllyCount = 0;
            this.TalismanCount = 0;
            this.WeaponCount = 0;
            this.TotemCount = 0;
            this.GoldCount = 0;
            this.GoldCosts = new int[8];
        }
        public List<Carta> CardsInDeck { get; set; }
        public int DeckCount { get; set; }
        public int AllyCount { get; set; }
        public int TalismanCount { get; set; }
        public int WeaponCount { get; set; }
        public int TotemCount { get; set; }
        public int GoldCount { get; set; }
        public int[] GoldCosts { get; set; }

        public List<ChartItem> DistributionData { get; set; }
        public List<ChartItem> GoldCurve { get; set; }

        public void UpdateCharts()
        {
            DistributionData = new List<ChartItem>
            {
                new ChartItem() { Name = "Aliados", Value = AllyCount },
                new ChartItem() { Name = "Talismanes", Value = TalismanCount },
                new ChartItem() { Name = "Armas", Value = WeaponCount },
                new ChartItem() { Name = "Tótems", Value = TotemCount },
                new ChartItem() { Name = "Oros", Value = GoldCount }
            };

            GoldCurve = new List<ChartItem>
            {
                new ChartItem() { Name = "0", Value = GoldCosts[0] },
                new ChartItem() { Name = "1", Value = GoldCosts[1] },
                new ChartItem() { Name = "2", Value = GoldCosts[2] },
                new ChartItem() { Name = "3", Value = GoldCosts[3] },
                new ChartItem() { Name = "4", Value = GoldCosts[4] },
                new ChartItem() { Name = "5", Value = GoldCosts[5] },
                new ChartItem() { Name = "6", Value = GoldCosts[6] },
                new ChartItem() { Name = "7+", Value = GoldCosts[7] },
            };
        }
    }
}
