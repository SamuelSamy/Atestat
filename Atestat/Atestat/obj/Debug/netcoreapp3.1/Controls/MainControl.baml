   M S B A M L   `   `   `  ����B  >Atestat, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:AtestatAtestat  5          Atestat.MainControl   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      'localclr-namespace:Atestat     5        � 	Resources  5
       �� Source�� 5   
   $. (/Atestat;component/Resources/Styles.xaml?�6   %    .��5      � 
������� � ColumnDefinitions 5   
   �� 5      $
��229* �6   �� 5      $
��566* �6   �� 5      $��5* �6   .+�5   
   � -   5         �Name$ sideBarMain��5      $y�Visible=�6$      
WhiteBrush#��[  6:   $��0��6c    � RowDefinitions 5      �� 5      $��1* �6    �� 5      $��1* �6    �� 5      $��1* �6    �� 5      $��1* �6    �� 5      $��1* �6    �� 5      $��1* �6    �� 5      $��1* �6     5!      �� 5"      $��1* �6#   �� 5#      $��6* �6#   �� 5$      $��1* �6#   .+�5'      �� -   5         ��Name$ btnAnunturi��5'      $��1��5(      $��1��5)        RoundButton#��[ 5*      $��Anunturi��5,      
�������$��36(�5.      $
��Bold"�5/      $��Stretch=�6'   $��Stretch=�6E   
 � Tag$ 36��6�    J� Effect 51      3  -System.Windows.Media.Effects.DropShadowEffect  52         
BlurRadius$ 10Q�6'   	   ShadowDepth$	 10Q�67   54      �� 56      -   5        $ btnLogin��56      $��3��57      $��1��58        LightBlueBrush#��[ 59      #��[ 5:      $��	Conectare��5<      
�������$��36(�5>      $
��Bold"�5?      $ 36��5@   6    5D        5E      $ 10Q�6'   $	 10Q�67   5H      �� 5J      -   5        $ btnHelp��5J      $��5��5K      $��1��5L      #��[ 5M      #��[ 5N      $��Ajutor��5P      
�������$��36(�5R      $
��Bold"�5S      $ 36��5T   L    5V        5W      $ 10Q�6'   $	 10Q�67   5Z      5]      � 5_   
   -   5        $ gridLogo��5_      $��1��6!   $y�Visible=�61   $��2��6F    5a      �� 5b      $��1* �6    �� 5c      $��5* �6    �� 5d      $��1* �6     5g      �� 5h      $��1* �6#   �� 5i      $��5* �6#   �� 5j      $��1* �6#   .+�5m      �� $��1��6   $��1��6!   $��/Assets/RoundLogo.png��61   
 J� RenderTransformOrigin$
 0.5,0.5&�6P    5n        5o      $ 10Q�6'   $	 15Q�67   z�5q      V� 5r      .~�5s      �� �� 5t      �� 5u       �� Angle$ 30Q�6*   T� 5v      5w      5y      5{      5}      5      