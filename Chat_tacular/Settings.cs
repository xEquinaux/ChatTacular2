using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

namespace Chat_tacular;

public partial class Settings : Window, IComponentConnector
{
	internal const byte TAB_BG = 0;

	internal const byte TAB_TEXT = 1;

	internal static bool[] ActiveTab = new bool[2];

	internal static Settings Instance;

	internal ImageBase Base => App.Chat;

	public Settings()
	{
		InitializeComponent();
		Instance = this;
		foreach (FontFamily fontFamily in Fonts.GetFontFamilies(Environment.GetEnvironmentVariable("WINDIR") + "\\Fonts"))
		{
			string text = fontFamily.ToString();
			list_fonts.Items.Add(text.Substring(text.IndexOf("#") + 1));
		}
		foreach (FontFamily fontFamily2 in Fonts.GetFontFamilies(Environment.GetEnvironmentVariable("WINDIR") + "\\Fonts"))
		{
			list_fonts_real.Items.Add(fontFamily2);
		}
	}

	private void Button_Click(object sender, RoutedEventArgs e)
	{
		if (radio_color.IsChecked.Value)
		{
			ColorDialog colorDialog = new ColorDialog();
			if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				string name = colorDialog.Color.Name;
				if (name.ToLower().StartsWith("ff"))
				{
					name = "#" + name;
				}
				Base.bg_color.Dispatcher.BeginInvoke((ThreadStart)delegate
				{
					Base.bg_color.Fill = (Brush)new BrushConverter().ConvertFromString(name);
					Base.bg_color.Stroke = (Brush)new BrushConverter().ConvertFromString(name);
				});
			}
		}
		else if (radio_image.IsChecked.Value)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Filter = "All Files (*.*)|*.*|PNG Files (*.png)|*.png|JPG Files (*.jpg,*.jpeg)|*.jpg;*.jpeg|BMP Files (*.bmp)|*.bmp|GIF Files (*.gif)|*.gif";
			dialog.Multiselect = false;
			dialog.Title = "Select background image";
			dialog.InitialDirectory = Environment.GetEnvironmentVariable("USERPROFILE");
			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				Base.bg_image.Dispatcher.BeginInvoke((ThreadStart)delegate
				{
					ImageBase.ImageFile = dialog.FileName;
					Base.bg_image.Source = new BitmapImage(new Uri(dialog.FileName));
					Base.matte_image.Source = new BitmapImage(new Uri(dialog.FileName));
				});
			}
		}
		else
		{
			if (!radio_media.IsChecked.Value)
			{
				return;
			}
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Filter = "All Files (*.*)|*.*|Media Files (*.MKV,*.MP4,*.AVI,*.WMA,*.MPG,*.MPEG)|*.MKV;*.MP4;*.AVI;*.WMA;*.MPG;*.MPEG";
			dialog.Multiselect = false;
			dialog.Title = "Select media file";
			dialog.InitialDirectory = Environment.GetEnvironmentVariable("USERPROFILE");
			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				Base.bg_media.Dispatcher.BeginInvoke((ThreadStart)delegate
				{
					Base.bg_media.Source = new Uri(dialog.FileName);
					Base.bg_media.Play();
				});
			}
		}
	}

	private void list_fonts_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		list_fonts_real.SelectedIndex = list_fonts.SelectedIndex;
		FontFamily font = (FontFamily)list_fonts_real.SelectedItem;
		App.Chat.Dispatcher.BeginInvoke((ThreadStart)delegate
		{
			App.Chat.text_chat.FontFamily = font;
			App.Chat.text_chat_bg.FontFamily = font;
			Login.ChatFont = font.Source.Substring(font.Source.IndexOf('#') + 1);
		});
	}

	private void tab_control_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		for (int i = 0; i < ActiveTab.Length; i++)
		{
			ActiveTab[i] = false;
		}
		ActiveTab[tab_control.SelectedIndex] = true;
	}

	private void BgColor_Checked(object sender, RoutedEventArgs e)
	{
		Base?.Dispatcher.BeginInvoke((ThreadStart)delegate
		{
			Base.bg_color.Visibility = Visibility.Visible;
			Base.bg_image.Visibility = Visibility.Hidden;
			Base.matte_image.Visibility = Visibility.Hidden;
			Base.bg_media.Visibility = Visibility.Hidden;
			Base.bg_media.Stop();
		});
	}

	private void BgImage_Checked(object sender, RoutedEventArgs e)
	{
		Base?.Dispatcher.BeginInvoke((ThreadStart)delegate
		{
			Base.bg_color.Visibility = Visibility.Hidden;
			Base.bg_image.Visibility = Visibility.Visible;
			Base.matte_image.Visibility = Visibility.Visible;
			Base.bg_media.Visibility = Visibility.Hidden;
			Base.bg_media.Stop();
		});
	}

	private void BgMedia_Checked(object sender, RoutedEventArgs e)
	{
		Base?.Dispatcher.BeginInvoke((ThreadStart)delegate
		{
			Base.bg_color.Visibility = Visibility.Hidden;
			Base.bg_image.Visibility = Visibility.Hidden;
			Base.matte_image.Visibility = Visibility.Hidden;
			Base.bg_media.Visibility = Visibility.Visible;
			Base.bg_media.Play();
		});
	}

	private void slider_blurradius_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
	{
		Base.Dispatcher.BeginInvoke((ThreadStart)delegate
		{
			Base.bg_image.Effect = new BlurEffect
			{
				Radius = e.NewValue,
				RenderingBias = RenderingBias.Quality
			};
			Base.matte_image.Effect = new BlurEffect
			{
				Radius = e.NewValue + 4.0,
				RenderingBias = RenderingBias.Quality
			};
			Base.bg_media.Effect = new BlurEffect
			{
				Radius = e.NewValue,
				RenderingBias = RenderingBias.Quality
			};
		});
	}

	private void box_colors_bg_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		string text = box_colors_bg.SelectedValue.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "");
		ImageBase.BG_Text = (Brush)new BrushConverter().ConvertFromString(text);
		Login.BgChatFontColor = text;
	}

	private void box_colors_fg_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		string text = box_colors_fg.SelectedValue.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "");
		ImageBase.FG_Text = (Brush)new BrushConverter().ConvertFromString(text);
		Login.FgChatFontColor = text;
	}

	private void Glow_Checked(object sender, RoutedEventArgs e)
	{
		Base?.Dispatcher.BeginInvoke((ThreadStart)delegate
		{
			Base.richtextbox_bg.Visibility = Visibility.Visible;
			Base.richtextbox_bg.Opacity = 1.0;
			Base.richtextbox_bg.Margin = Base.richtextbox.Margin;
		});
	}

	private void Shadow_Checked(object sender, RoutedEventArgs e)
	{
		Base?.Dispatcher.BeginInvoke((ThreadStart)delegate
		{
			Base.richtextbox_bg.Visibility = Visibility.Visible;
			Base.richtextbox_bg.Opacity = 0.6;
			Base.richtextbox_bg.Margin = new Thickness(Base.richtextbox.Margin.Left + 4.0, Base.richtextbox.Margin.Top + 4.0, 0.0, 0.0);
		});
	}

	private void None_Checked(object sender, RoutedEventArgs e)
	{
		Base?.Dispatcher.BeginInvoke((ThreadStart)delegate
		{
			Base.richtextbox_bg.Visibility = Visibility.Hidden;
		});
	}

	private void box_fontsize_TextChanged(object sender, TextChangedEventArgs e)
	{
		if (int.TryParse(box_fontsize.Text, out var result) && result > 0)
		{
			Base.text_chat.FontSize = result;
			Base.text_chat_bg.FontSize = result;
		}
	}

	private void CheckBox_Click(object sender, RoutedEventArgs e)
	{
		Base.bg_bluepane.Opacity = (((System.Windows.Controls.CheckBox)sender).IsChecked.Value ? 1.0 : 0.0);
	}

	private void slider_chatwidth_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
	{
		ImageBase.MarginLeft = (float)e.NewValue;
		Base?.Window_SizeChanged(null, null);
	}

	private void Button_Click_1(object sender, RoutedEventArgs e)
	{
		Base.Dispatcher.Invoke(delegate
		{
			Base.text_chat_bg.Inlines.Clear();
			Base.text_chat.Inlines.Clear();
		});
	}

	internal static int GetItemIndex(string text, ItemCollection item)
	{
		int result = -1;
		for (int i = 0; i < item.Count; i++)
		{
			if (item[i].ToString().Contains(text))
			{
				result = i;
				break;
			}
		}
		return result;
	}

	private void Window_Loaded(object sender, RoutedEventArgs e)
	{
		if (Login.FgChatFontColor.Length > 0)
		{
			box_colors_fg.SelectedIndex = GetItemIndex(Login.FgChatFontColor, box_colors_fg.Items);
		}
		if (Login.BgChatFontColor.Length > 0)
		{
			box_colors_bg.SelectedIndex = GetItemIndex(Login.BgChatFontColor, box_colors_bg.Items);
		}
	}

	private void Window_Closing(object sender, CancelEventArgs e)
	{
		System.Windows.Application.Current.Shutdown();
	}
}
