   M S B A M L   `   `   `  ����D  @ClientApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:ClientApp	ClientApp  5   	   C&clr-namespace:ClientApp.MVVM.ViewModelClientApp.MVVM.ViewModel  5   	       ClientApp.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:ClientApp     5   	   <	viewModel&clr-namespace:ClientApp.MVVM.ViewModel     5   	   $	��600��5	   	   $	��920��6     � WindowStyle$
  None=�5
   	    � 
ResizeMode$ NoResize=�5   	   
������  � AllowsTransparency$
 True��5   	    � DataContext 5      ,  &ClientApp.MVVM.ViewModel.MainViewModel  5   
   .��5      �� ���#272537 �� CornerRadius$ 10��5      ._�5   
   �  � ColumnDefinitions 5      �� 5      $	��200 �6#   �� 5       � RowDefinitions 5      �� 5      $��75 �6    �� 5      .+�5!      ��  �� Orientation$ 
Horizontal=�6   .+�5"      �� $,��&/Images/Icons/50x50/icons8-home-50.png��6   $��35��5#      $��35��5$      $��Center=�5%      $��Center=�5&      $��30,5,0,0q�5'      �� 5(      $��Manager��6   $��24(�5)      $
��Bold"�5*      
�������$��Center=�5,      $��Center=�5-      $��	10,12,0,0q�5.      50      �� 52      $��1��6   .+�54      	� $
��Home��6   $��50��55      
�������$��14(�57         MenuButtonTheme#��[  58       d� 	IsChecked$
 TrueY�59      ��5:      �� *HomeViewCommand+	� 5<      $��
Properties��6   $��50��5=      
�������$��14(�5?      #��[  5@      ��5A      �� *PropertiesViewCommand+	� 5C      $��Notes��6   $��50��5D      
�������$��14(�5F      #��[  5G      ��5H      �� *NoteViewCommand+	� 5J      $��Gallary��6   $��50��5K      
�������$��14(�5M      #��[  5N      ��5O      �� *GallaryViewCommand+	� 5Q      $��Go Back��6   $��50��5R      
�������$��14(�5T      #��[  5U      $��	0,250,0,0q�5X      ��5V      �� *BackCommand+{�5W      �� *	CanGoBack+5X      5Z      �� 5\      $��1��6   $��1��5]      $��10q�5^      ��5_      �� *CurrentView+5b      5d      5e      