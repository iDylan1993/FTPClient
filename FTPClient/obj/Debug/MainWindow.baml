   M S B A M L   `   `   `  ����D  @FTPClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:FTPClient	FTPClient  5   	       FTPClient.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:FTPClient     5   	   -   5          � Title$  Server Client��5   	   $	��565��6   $	��805��6,    � 
ResizeMode$ NoResize=�6O    � WindowStartupLocation$ CenterScreen=�6e    � Topmost . .��5	      � .+�5
   
   �� -   5         ��Name$ FTPLabel��5
      $
��Left=�6&   $��13,9,0,0q�6A   $
��Wrap=�6S   $
��FTP:��6g   $	��Top=�6s   �� 5   
   -   5         ��Name$ FTPInput��5      $
��Left=�6%   $��23��6@   $��42,7,0,0q�6L    �� TextWrapping$ NoWrap=�6^   $	��Top=�6t   $	��140��6�   �� 5   
   -   5        $ UsernameLabel��5      $
��Left=�6+   $��	192,9,0,0q�6F   $
��Wrap=�6Y   $��	Username:��6m   $	��Top=�6~   �� 5   
   -   5        $ UsernameInput��5      $
��Left=�6)   $��23��6D   $��	258,7,0,0q�6P   $ NoWrap=�6c   $	��Top=�6y   $	��140��6�   �� 5   
   -   5        $ PasswordLabel��5      $
��Left=�6+   $��	409,9,0,0q�6F   $
��Wrap=�6Y   $��	Password:��6m   $	��Top=�6~   G� 5   
   -   5         G�Name$ PasswordInput��5      $
��Left=�6-   $��23��6H   $��	471,7,0,0q�6T   $	��Top=�6h   $	��140��6�   �� 5   
   -   5         ��Name$ ConnectButton��5      $��Connect��6(   $��	623,8,0,0q�6:   $	��Top=�6M   $
��Left=�6e   $��75��6�   �� 5   
   -	   5        $ ClearButton��5      $��Clear��6&   $��	704,8,0,0q�66   $	��Top=�6I   $
��Left=�6a   $��75��6|   �� 5   
   -
   5        	 ��Name$	 ServerFiles��5      $
��Left=�6(   $	��362��6C   $��	13,40,0,0q�6P   $	��Top=�6c   $	��766��6{   �� 5   
   $��Download from the server��6   $
��Left=�65   $��
13,405,0,0q�6P   $	��366��6d   $	��110��6p   $	��Top=�6}   .��5      �� $��-6,-15,-2,-5q�6   .+�5      �� -   5        $ DownloadLocationInput��5      $��23��69   
 �� Left$
 65��6E   $ NoWrap=�6V   {�.  
 �� Top$ 51��6~   $	��286��6�   �� 5      -   5        $ DownloadFileInput��5      $��23��65   $
 65��6A   $ NoWrap=�6R   {�.  $ 23��6{   $	��286��6�   �� 5      -   5        $ FileDownloadText��5      $
 10��66   $
��Wrap=�6G   $��File:��6[   $ 25��6h   �� 5      -   5        $ FileLocationText��5      $
 10��66   $
��Wrap=�6G   $��	Location:��6[   $ 54��6l   �� 5      -   5        $ DownloadButton��5      $��Download��61   {�.  $	
 276��6V   $ 79��6h   $��75��6x   �� 5      -   5        $ DownloadLocationButton��5      $��Change Location��69   {�.  $	
 174��6e   $ 79��6w   $��97��6�   �� 5      -   5        $ StatusDownloadLabel��5      $
 10��69   $
��Wrap=�6J   $��Nothing to download��6^   $ 83��6y   5      5      �� 5   
   $��Upload to the server��6   $
��Left=�61   $��413,405,0,0q�6L   $	��366��6a   $	��110��6m   $	��Top=�6z   .��5      �� $��-6,-15,-2,-5q�6   .+�5       �� -   5        $ UploadLocationInput��5       $��23��67   $
 65��6C   $ NoWrap=�6T   {�.  $ 51��6|   $	��286��6�   �� 5!      -   5        $ UploadFileInput��5!      $��23��63   $
 65��6?   $ NoWrap=�6P   {�.  $ 23��6y   $	��286��6�   �� 5"      -   5        $ FileUploadText��5"      $
 10��64   $
��Wrap=�6E   $��File:��6Y   $ 25��6f   �� 5#      -   5        $ FileUploadLocationText��5#      $
 10��6<   $
��Wrap=�6M   $��	Location:��6a   $ 54��6r   �� 5$      -   5        $ UploadButton��5$      $��Upload��6/   {�.  $	
 276��6R   $ 79��6d   $��75��6t   �� 5%      -   5        $ UploadLocationButton��5%      $��Change Location��67   {�.  $	
 174��6c   $ 79��6u   $��97��6�   �� 5&      -   5        $ StatusUploadLabel��5&      $
 10��67   $
��Wrap=�6H   $��Nothing to upload��6\   $ 83��6u   5'      5(      5)      5*      