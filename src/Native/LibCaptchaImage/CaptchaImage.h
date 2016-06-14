// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the WIN32PROJECT1_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// WIN32PROJECT1_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef PROJECT_EXPORTS
#define Export_API extern "C" __declspec(dllexport)
#else
#define Export_API extern "C"
#endif



Export_API void GCaptcha(char* file_o, char *captcha_text, int count, int width, int height, int offset, int quality, int isjpeg, int fontSize);
