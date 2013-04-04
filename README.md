NinePatch Resizer
===============

While developing for Android, in order to support multiple screen densities you need to provide 4 different versions of each image (drawable) for each density category. When working with simple images (png, jpg) this task is pretty simple. But when it gets to 9-patch images things become complicated.

If you'll try to resize 9-patch image (.9.png) in your favorite editor, you'll notice that black pixels at the edges of the image become blurred (which means that this 9-patch won't be compilled correctly). And you have no other choice to resize image and draw patches again, and again...

NinePatch Resizer can do all of this for you. It correctly resizes your 9-patch images (or a simple images) for each density and arranges result into corresponding folders (drawable-xhdpi, etc.).


[Download](https://github.com/anastasia-zaitsewa/NinePatchResizer/blob/master/NinePatchResizer.zip?raw=true)

Usage
=====

Just drag and drop folder with your images (or a single image) into application. Result will be available inside dropped folder (or in the same directory as image).

License
=======

  Copyright anastasia.zaitsewa@gmail.com Anastasia Zaitseva

	Licensed under the Apache License, Version 2.0 (the "License");
	you may not use this file except in compliance with the License.
	You may obtain a copy of the License at

		http://www.apache.org/licenses/LICENSE-2.0

	Unless required by applicable law or agreed to in writing, software
	distributed under the License is distributed on an "AS IS" BASIS,
	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	See the License for the specific language governing permissions and
	limitations under the License.
