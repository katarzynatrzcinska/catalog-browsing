   M S B A M L   `   `   `  ����J  FWpfApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:WpfApplication1WpfApplication1  5   	   "    WpfApplication1.CreateDialog   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:WpfApplication1     5   	     � Title$  CreateDialog��5   	   $	��400��6   $	��300��6+   .��5	      �  � RowDefinitions 5
   
   �� 5      $
��190* �6   �� 5      $	��79* �6    � ColumnDefinitions 5   
   �� 5      $	��79* �6   �� 5      $	��67* �6   .+�5   
   �� -   5        $��lblQuestion��5      $��2��6#   $��	0,0,0,158q�67   .��6J   Name:6Q   �� 5   
   -   5         ��Name$ 
textAnswer��5      $��32,32,23,188q�6&   $��2��6<   .��6P   Name6V   	� 5   
   -   5         	�Name$ 
createFile��5      $
��File��6*    d� 	IsChecked$
 TrueY�69   $��2��6J   $��59,91,75,146q�6^   	� 5   
   -   5        $ createDirectory��5      $��	Directory��6/   $��2��6C   $��59,115,75,125q�6W   �� 5   
   -   5         ��Name$ readOnly��5      $��46,173,96,67q�6%   $��ReadOnly��6;   $��2��6N   �� 5   
   -   5        $ archive��5      $��46,194,96,46q�6$   $��Archive��6:   $��2��6L   �� 5   
   -   5        $ hidden��5      $��46,215,96,25q�6#   $��Hidden��69   $��2��6J   �� 5   
   -   5        $ system��5      $��46,236,96,4q�6#   $��System��68   $��2��6I   �� 5   
   -	   5        $��Ok��5      $��75,58,24,27q�6   $��1��63   6@   �� 5   
   -
   5        $��Cancel��5      $��
0,58,75,27q�6"   $��1��66   $��1��6C   6S   5      5      