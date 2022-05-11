# NetCheat PS3 4.53

Written by Dnawrkshp

## Comments

This version differs slightly from the released version of NetCheat 4.53 in the following ways:

 - Removed online PS3 codelist browser
 - Removed donation garbage

## Compile

	1. Install Visual Studio 2019 with .NET Desktop Development package
	2. Open Solution in Visual Studio
	3. Build

## Plugins

	Plugins allow third-party developers to add new features to NetCheat. They are accessible from the Plugins tab.
	
Checkout [NCMemBrowser](./NCMemBrowser/Plugin.cs) for an example.

## APIs

	Apis are plugins that extend what platforms you can use with NetCheat. APIs can be selected in the APIs tab. NetCheat currently ships with an API for PS3 and PC. Other third-party APIs may exist somewhere on the internet.

Checkout [PCAPI](./PCAPI-NCAPI/API.cs) for an example.
	
## Tips

	- Double click on a plugin to open/close it
	- Double click a code to toggle constant write
	- Ctrl-C on result(s) will copy them into the clipboard in code format
	- Search results do not automatically refresh
	- When you test a subroutine and you want to update it after already writing it, click Reset Written. Then you can replace the code and write again without a possible freeze


If you want to make a plugin I suggest watching my tutorial on how:
	http://youtu.be/ySDr5H6VD58
	
Enjoy.
