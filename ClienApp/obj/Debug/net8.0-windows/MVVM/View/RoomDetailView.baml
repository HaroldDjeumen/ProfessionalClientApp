   M S B A M L   `   `   `  ����D  @ClientApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:ClientApp.MVVM.ViewClientApp.MVVM.View  5      C&clr-namespace:ClientApp.MVVM.ViewModelClientApp.MVVM.ViewModel  5      (    "ClientApp.MVVM.View.RoomDetailView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      3local!clr-namespace:ClientApp.MVVM.View     5      5vm&clr-namespace:ClientApp.MVVM.ViewModel     5      .��5      �   � ColumnDefinitions  5   
   �� 5      $��3* �6   �� 5      $��3* �6   .+�5      �� $��0��6   $	��320��5      $	��485��5       �� CornerRadius$ 10��5      
������  J� OpacityMask 5      !� 5       j� Stretch$
 None=�6"    !� Visual 5      �� 5      $ 	10,0,0,10��6%   $	��320��5   %   $	��485��5   %   
���   �5   %   5       ._�5#      �� $	��320��5$      $	��485��5%      $
��Fill=�5&      ��5#      �� *CurrentRoom.RoomImage+5&      5'      �� 5+   
   $��1��6   $��Center=�6%   $	��490��6@   .+�5-      �� $��26(�6>   $
��Bold"�6L   $��30��6^   
�������$��10,0,0,0q�6~   ��6   �� *CurrentRoom.RoomName+6~   �� 5.      $��16(�6>   $��25��6z   $��10,5,0,0q�6�   ��6   �� *CurrentRoom.RoomType+��6L   �� *CurrentRoom.Lightcolor+6�   �� 50      $��Status:��6   $
��Bold"�6'   $��	10,10,0,0q�69   
�������6L   �� 51      $��10,5,0,0q�6<   
���������6   �� *CurrentRoom.Status+6N   �� 53      $��Tenant:��6   $
��Bold"�6'   $��	10,10,0,0q�69   
�������6L   �� 54      $��10,5,0,0q�6@   
���������6   �� *CurrentRoom.TenantName+6R   �� 56      $��Stay Duration:��6   $
��Bold"�6.   $��	10,10,0,0q�6@   
�������6S   �� 57      $��10,5,0,0q�6@   
���������6   �� *CurrentRoom.StayPeriod+6R   �� 59      $��Rent Status:��6   $
��Bold"�6,   $��	10,10,0,0q�6>   
�������6Q   �� 5:      $��10,5,0,0q�6D   
���������6   �� *CurrentRoom.RentPaidStatus+6V   �� 5=      
������ 
������ $��0q�5?      $��Center=�5@      ��5B      �� 5C       �� 
TargetType" ��6   .�5D      ��  �� Property � �� �� Value
 ���� 67   �� 5E       � ��
 ���� 68   �� 5F       �@�OverridesDefaultStyle$
 True��6B   �� 5G       � �� 5H      �� 5I   "    �� 
TargetType" ��62   .R�5J   &   �� 5K   #   5M      5N      .��5Q      �� $	��105��6   $��30��5R      $ 1��5S      $��5,10,0,0q�5T      ��5U      �� 5V      	 �� 
StartPoint$		 0,0&�6.   
 �� EndPoint$	
 5,1&�6?   .��5W      �  � Offset$	 0.0Q�6S    � Color 6+   �� *CurrentRoom.Darkcolor+6S   � 5X      $	 1.0Q�6T    6+   �� *CurrentRoom.Lightcolor+6T   5Y       J� Clip 5[      �� 5\       �� RadiusX$ 5Q�6,    �� RadiusY$ 5Q�5]   ,    �� Rect$ 
0,0,105,30�5^   ,   ._�5`      �� $��	Edit info��6!   
�������$��15(�5b   !   $��Center=�5c   !   $��Center=�5d   !   5e      5f      �� 5h      
������ $	��330��5i      $	��191��5j      $��0,10,0,5q�5k       �� Padding$ 2q�5l      $��2q�5n      $ 0,0,10,0��5o      ��5m      �� *CurrentRoom.Lightcolor+5o      5p      5q      5r      