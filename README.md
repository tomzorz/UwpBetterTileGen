# UwpBetterTileGen
A (better) tile generator for Universal Windows Apps

I made this because I wasn't satisfied with the results of the tile generators available today. (today = 2016.02.11.)

## Features

This command line tool currently generates all the visual assets that a UWP app can have (except badges, but I'll add that later).

## Usage

As of version 1.0 you'll need to prepare four .png images as source material:

* wide.png (1240x600)
* splash.png (2480x1200)
* square_large.png (1240x1240)
* square_small.png (284x284)

Place these files into the folder where the *UwpBetterTileGen.exe* and the *ImageProcessor.dll* are, and just simply start the program. You'll find the results in a new folder called *output*.

## Future

There are plenty of places to improve:

* better image resizing
* ability to use different source for alternate tiles
* single source file generation
* generation of not regular asset files
* automatically editing appxmanifest
* badge asset support