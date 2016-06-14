// Win32Project1.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "CaptchaImage.h"
#include "CImg.h"


using namespace cimg_library;

// This is an example of an exported function.
Export_API void GCaptcha(char* file_o, char *captcha_text, int count, int width ,int height , int offset ,int quality, int isjpeg, int fontSize)
{
	// Create captcha image
	//----------------------
	// Write colored and distorted text
	CImg<unsigned char> captcha(width, height, 1, 3, 0), color(3);
	const unsigned char red[] = { 255,0,0 }, green[] = { 0,255,0 }, blue[] = { 0,0,255 };

	char letter[2] = { 0 };
	for (unsigned int k = 0; k<count; ++k) {
		CImg<unsigned char> tmp;
		*letter = captcha_text[k];
		if (*letter) {
			cimg_forX(color, i) color[i] = (unsigned char)(128 + (std::rand() % 127));
			tmp.draw_text((int)(2 + 8 * cimg::rand()),
						  (int)(12 * cimg::rand()), letter, red, 0, 1, fontSize).resize(-100, -100, 1, 3);

			const float sin_offset = (float)cimg::crand() * 3, sin_freq = (float)cimg::crand() / 7;
			
			cimg_forYC(captcha, y, v) captcha.get_shared_row(y, 0, v).shift((int)(4 * std::cos(y*sin_freq + sin_offset)));
			
			captcha.draw_image(count + offset * k, tmp);
		}
	}

	// Add geometric and random noise
	CImg<unsigned char> copy = (+captcha).fill(0);
	for (unsigned int l = 0; l<3; ++l) {
		if (l) copy.blur(0.5f).normalize(0, 148);
		for (unsigned int k = 0; k<10; ++k) {
			cimg_forX(color, i) color[i] = (unsigned char)(128 + cimg::rand() * 127);
			if (cimg::rand() < 0.5f) {
				copy.draw_circle((int)(cimg::rand()*captcha.width()),
									 (int)(cimg::rand()*captcha.height()),
									 (int)(cimg::rand() * 30),
									 color.data(), 0.6f, ~0U);
			}
			else {
				copy.draw_line((int)(cimg::rand()*captcha.width()),
									(int)(cimg::rand()*captcha.height()),
									(int)(cimg::rand()*captcha.width()),
									(int)(cimg::rand()*captcha.height()),
									color.data(), 0.6f);
			}
		}
	}
	captcha |= copy;
	captcha.noise(10, 2);

	captcha = (+captcha).fill(255) - captcha;

	// Write output image and captcha text
	//-------------------------------------
	//std::printf("%s\n",captcha_text);

	if (isjpeg) {
		captcha.save_jpeg(file_o, quality);
	}
	else {
		captcha.save(file_o);
	}

}

