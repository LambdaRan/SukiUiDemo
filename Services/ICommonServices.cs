using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SukiUiDemo.Services;

public interface ICommonServices<TModel>
{
	MessageService MessageService { get; }
	ILogger<TModel> Logger { get; }
}
