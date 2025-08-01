﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Launcher.View.Components
{
    public partial class CartGame : UserControl
    {
        public static readonly DependencyProperty SetIconProperty =
            DependencyProperty.Register("SetIcon", typeof(BitmapImage), typeof(CartGame), new PropertyMetadata(null));

        public static readonly DependencyProperty SetNameProperty =
            DependencyProperty.Register("SetName", typeof(string), typeof(CartGame), new PropertyMetadata(string.Empty));

        public BitmapImage SetIcon
        {
            get => (BitmapImage)GetValue(SetIconProperty);
            set => SetValue(SetIconProperty, value);
        }

        public string SetName
        {
            get => (string)GetValue(SetNameProperty);
            set => SetValue(SetNameProperty, value);
        }

        public CartGame()
        {
            InitializeComponent();
        }
    }
}
