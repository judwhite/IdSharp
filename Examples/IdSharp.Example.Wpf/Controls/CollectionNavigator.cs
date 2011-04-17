using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using IdSharp.Common.Events;

namespace IdSharp.Tagging.Harness.Wpf.Controls
{
    public class CollectionNavigator : Control
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IList), typeof(CollectionNavigator), new PropertyMetadata(OnItemsSource));
        public static readonly DependencyProperty CurrentPositionProperty = DependencyProperty.Register("CurrentPosition", typeof(int?), typeof(CollectionNavigator), new PropertyMetadata(OnCurrentPositionChanged));
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(CollectionNavigator));

        static CollectionNavigator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CollectionNavigator), new FrameworkPropertyMetadata(typeof(CollectionNavigator)));
        }

        private static void OnItemsSource(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CollectionNavigator)d).ItemsSourceChanged();
        }

        private static void OnCurrentPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CollectionNavigator)d).CurrentPositionChanged();
        }

        public event EventHandler<CancelDataEventArgs<object>> BeforeAdd;
        public event EventHandler<CancelDataEventArgs<object>> BeforeDelete;

        private ButtonBase FirstButton;
        private ButtonBase PreviousButton;
        private ButtonBase NextButton;
        private ButtonBase LastButton;
        private ButtonBase DeleteButton;
        private ButtonBase AddButton;
        private TextBox CurrentPositionTextBox;
        private TextBlock ItemCountTextBlock;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            FirstButton = (ButtonBase)GetTemplateChild("FirstButton");
            PreviousButton = (ButtonBase)GetTemplateChild("PreviousButton");
            NextButton = (ButtonBase)GetTemplateChild("NextButton");
            LastButton = (ButtonBase)GetTemplateChild("LastButton");
            AddButton = (ButtonBase)GetTemplateChild("AddButton");
            DeleteButton = (ButtonBase)GetTemplateChild("DeleteButton");
            CurrentPositionTextBox = (TextBox)GetTemplateChild("CurrentPositionTextBox");
            ItemCountTextBlock = (TextBlock)GetTemplateChild("ItemCountTextBlock");

            // ReSharper disable PossibleNullReferenceException
            FirstButton.Click += delegate { CurrentPosition = 1; };
            PreviousButton.Click += delegate { CurrentPosition--; };
            NextButton.Click += delegate { CurrentPosition++; };
            LastButton.Click += delegate { CurrentPosition = ItemsSource.Count; };
            DeleteButton.Click += DeleteButton_Click;
            AddButton.Click += AddButton_Click;
            // ReSharper restore PossibleNullReferenceException
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var handler = BeforeAdd;
            if (handler == null)
                return;

            var collection = ItemsSource;

            CancelDataEventArgs<object> eventArgs = new CancelDataEventArgs<object>(null);
            handler(this, eventArgs);
            if (eventArgs.Cancel || eventArgs.Data == null)
                return;

            collection.Add(eventArgs.Data);
            ItemsSourceChanged();
            CurrentPosition = collection.Count;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPosition == null)
                return;

            var handler = BeforeDelete;
            if (handler == null)
                return;

            var collection = ItemsSource;
            int index = CurrentPosition.Value - 1;
            object item = collection[index];

            CancelDataEventArgs<object> eventArgs = new CancelDataEventArgs<object>(item);
            handler(this, eventArgs);
            if (eventArgs.Cancel)
                return;

            collection.RemoveAt(index);

            ItemsSourceChanged();
        }

        public IList ItemsSource
        {
            get { return (IList)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public int? CurrentPosition
        {
            get { return (int?)GetValue(CurrentPositionProperty); }
            set { SetValue(CurrentPositionProperty, value); }
        }

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        private void ItemsSourceChanged()
        {
            ICollection collection = ItemsSource;
            if (collection == null || collection.Count == 0)
            {
                CurrentPosition = null;
                ItemCountTextBlock.Text = string.Empty;
            }
            else
            {
                CurrentPosition = 1;
                ItemCountTextBlock.Text = string.Format("of {0:#,0}", collection.Count);
            }

            UpdateButtons();
        }

        private void CurrentPositionChanged()
        {
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            int? currentPosition = CurrentPosition;
            IList collection = ItemsSource;

            if (collection == null || collection.Count == 0)
            {
                FirstButton.IsEnabled = false;
                PreviousButton.IsEnabled = false;
                NextButton.IsEnabled = false;
                LastButton.IsEnabled = false;
                CurrentPositionTextBox.IsEnabled = false;
                DeleteButton.IsEnabled = false;
                AddButton.IsEnabled = collection != null;

                SelectedItem = null;                
                
                return;
            }

            CurrentPositionTextBox.IsEnabled = true;

            if (currentPosition > collection.Count)
            {
                CurrentPosition = collection.Count;
                return;
            }
            else if (currentPosition == null || currentPosition < 1)
            {
                CurrentPosition = 1;
                return;
            }

            SelectedItem = collection[currentPosition.Value - 1];

            FirstButton.IsEnabled = currentPosition != 1;
            PreviousButton.IsEnabled = currentPosition != 1;
            NextButton.IsEnabled = currentPosition < collection.Count;
            LastButton.IsEnabled = currentPosition < collection.Count;
            DeleteButton.IsEnabled = true;
            AddButton.IsEnabled = true;
        }
    }
}
