﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vax.Service.DTOS.RequestDto;
using Vax.Service.DTOS.ResponseDto;
using Vax.Service.Helper;

namespace Vax.Service.Interface
{
	public interface IAuthService
	{
		public Task<BaseResult<TokenDto>> LoginAsync(LoginDto loginDto);

		public Task<BaseResult<string>> RegisterAsync(RegisterDto registerDto,string accountype);
	}
}
