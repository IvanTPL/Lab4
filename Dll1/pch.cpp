#include "pch.h"
#include "mkl.h"
#include <string.h>
#include <time.h>
#include <stdio.h>
#include <cmath>
#include <iostream>
#include <chrono>
#include <ctime> 

extern "C" _declspec(dllexport) int MKL_function(int n, double* arr, float* arr_float, float* res_time, int func, double* res_HA, double* res_EP, float* res_HA_float, float* res_EP_float)
{
	try
	{
		MKL_INT64 mode1 = VML_HA;
		MKL_INT64 mode2 = VML_EP;

		if (func == 0)
		{
			auto begin1 = std::chrono::system_clock::now();
			vmdLn(n, arr, res_HA, mode1);
			auto end1 = std::chrono::system_clock::now();
			std::chrono::duration<double> time1 = end1 - begin1;
			res_time[0] = time1.count();
			auto begin2 = std::chrono::system_clock::now();
			vmdLn(n, arr, res_EP, mode2);
			auto end2 = std::chrono::system_clock::now();
			std::chrono::duration<double> time2 = end2 - begin2;
			res_time[1] = time2.count();
			res_time[2] = res_time[1] / res_time[0];
		}
		else if (func == 1)
		{
			auto begin1 = std::chrono::system_clock::now();
			vmsLn(n, arr_float, res_HA_float, mode1);
			auto end1 = std::chrono::system_clock::now();
			std::chrono::duration<double> time1 = end1 - begin1;
			res_time[0] = time1.count();
			auto begin2 = std::chrono::system_clock::now();
			vmsLn(n, arr_float, res_EP_float, mode2);
			auto end2 = std::chrono::system_clock::now();
			std::chrono::duration<double> time2 = end2 - begin2;
			res_time[1] = time2.count();
			res_time[2] = res_time[1] / res_time[0];
		}
		else if (func == 2)
		{
			auto begin1 = std::chrono::system_clock::now();
			vmdLGamma(n, arr, res_HA, mode1);
			auto end1 = std::chrono::system_clock::now();
			std::chrono::duration<double> time1 = end1 - begin1;
			res_time[0] = time1.count();
			auto begin2 = std::chrono::system_clock::now();
			vmdLGamma(n, arr, res_EP, mode2);
			auto end2 = std::chrono::system_clock::now();
			std::chrono::duration<double> time2 = end2 - begin2;
			res_time[1] = time2.count();
			res_time[2] = res_time[1] / res_time[0];
		}
		else if (func == 3)
		{
			auto begin1 = std::chrono::system_clock::now();
			vmsLGamma(n, arr_float, res_HA_float, mode1);
			auto end1 = std::chrono::system_clock::now();
			std::chrono::duration<double> time1 = end1 - begin1;
			res_time[0] = time1.count();
			auto begin2 = std::chrono::system_clock::now();
			vmsLGamma(n, arr_float, res_EP_float, mode2);
			auto end2 = std::chrono::system_clock::now();
			std::chrono::duration<double> time2 = end2 - begin2;
			res_time[1] = time2.count();
			res_time[2] = res_time[1] / res_time[0];
		}
		return 0;
	}
	catch (...)
	{
		return -1;
	}
}
