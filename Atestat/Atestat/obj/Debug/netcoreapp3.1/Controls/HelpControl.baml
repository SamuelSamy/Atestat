   M S B A M L   `   `   `  ����B  >Atestat, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Atestat.ControlsAtestat.Controls  5      "    Atestat.Controls.HelpControl   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:Atestat.Controls     5        � 	Resources  5
       �� Source�� 5   
   $. (/Atestat;component/Resources/Styles.xaml?�6   %    .��5      � 
������� � ColumnDefinitions 5   
   �� 5      $
��229* �6   �� 5      $
��566* �6   �� 5      $��5* �6   .+�5   
   � -   5         �Name$ sideBarMain��5      $y�Visible=�6$      
WhiteBrush#��[  6:   $��0��6c    � RowDefinitions 5      �� 5      $��1* �6    �� 5      $��1* �6    �� 5      $��1* �6    �� 5      $��1* �6    �� 5      $��1* �6    �� 5      $��1* �6    �� 5      $��1* �6     5       �� 5!      $��1* �6#   �� 5"      $��6* �6#   �� 5#      $��1* �6#   .+�5&      �� -   5         ��Name$ btnTutorial��5&      $��1��5'      $��1��5(        LightBlueBrush#��[ 5)        RoundButton#��[ 5*      $��Tutorial��5,      
�������$��36(�5.      $
��Bold"�5/      $��Stretch=�6'   $��Stretch=�6E   
 � Tag$ 36��50   /    J� Effect 52      3  -System.Windows.Media.Effects.DropShadowEffect  53         
BlurRadius$ 10Q�6'   	   ShadowDepth$	 10Q�67   55      �� 57      -   5        $ 
btnContact��57      $��3��58      $��1��59      #��[ 5:      #��[ 5;      $��Contact��5=      
�������$��36(�5?      $
��Bold"�5@      $ 36��5A   O    5E        5F      $ 10Q�6'   $	 10Q�67   5I      �� 5K      -   5        $ btnBack��5K      $��5��5L      $��1��5M      #��[ 5N      #��[ 5O      $��Inapoi��5Q      
�������$��36(�5S      $
��Bold"�5T      $ 36��5U   L    5W        5X      $ 10Q�6'   $	 10Q�67   5[      5^      � 5`   
   -   5        $ gridLogo��5`      $��1��6!   $y�Visible=�61   $��2��6F    5b      �� 5c      $��1* �6    �� 5d      $��5* �6    �� 5e      $��1* �6     5h      �� 5i      $��1* �6#   �� 5j      $��5* �6#   �� 5k      $��1* �6#   .+�5n      �� $��1��6   $��1��6!   $��/Assets/RoundLogo.png��61   
 J� RenderTransformOrigin$
 0.5,0.5&�6P    5o        5p      $ 10Q�6'   $	 15Q�67   z�5r      V� 5s      .~�5t      �� �� 5u      �� 5v       �� Angle$ 30Q�6*   T� 5w      5x      5z      5|      5~      5�      