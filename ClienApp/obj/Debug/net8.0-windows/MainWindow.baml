   M S B A M L   `   `   `  ����C  ?ClienApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:ClienAppClienApp  5   	   A%clr-namespace:ClienApp.MVVM.ViewModelClienApp.MVVM.ViewModel  5   	       ClienApp.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:ClienApp     5   	   ;	viewModel%clr-namespace:ClienApp.MVVM.ViewModel     5   	   $	��600��5	   	   $	��920��6     � WindowStyle$
  None=�5
   	    � 
ResizeMode$ NoResize=�5   	   
������  � AllowsTransparency$
 True��5   	    � DataContext 5      +  %ClienApp.MVVM.ViewModel.MainViewModel  5   
   .��5      �� ���#272537 �� CornerRadius$ 10��5      ._�5   
   �  � ColumnDefinitions 5      �� 5      $	��200 �6#   �� 5       � RowDefinitions 5      �� 5      $��75 �6    �� 5      .+�5!      �� $,��&/Images/Icons/50x50/icons8-home-50.png��6   $��50��5"      $��50��5#      $��Center=�5$      $��Center=�5%      $��10q�5&      �� 5)      $��1��6   .+�5+      	� $
��Home��6   $��50��5,      
�������$��14(�5.         MenuButtonTheme#��[  5/       d� 	IsChecked$
 TrueY�50      ��51      �� *HomeViewCommand+	� 53      $��
Properties��6   $��50��54      
�������$��14(�56      #��[  57      ��58      �� *PropertiesViewCommand+	� 5:      $��Notes��6   $��50��5;      
�������$��14(�5=      #��[  5>      ��5?      �� *NoteViewCommand+	� 5A      $��Gallary��6   $��50��5B      
�������$��14(�5D      #��[  5E      ��5F      �� *GallaryViewCommand+	� 5H      $��Go Back��6   $��50��5I      
�������$��14(�5K      #��[  5L      $��	0,250,0,0q�5O      ��5M      �� *BackCommand+{�5N      �� *	CanGoBack+5O      5Q      �� 5S      $	��250��6   $��40��5T      $��Center=�5U      $
��Left=�5V      $��5q�5W      $��1��5X        ModernTextbox#��[ 5Y      �� 5[      $��1��6   $��1��5\      $��10q�5]      ��5^      �� *CurrentView+5a      5c      5d      