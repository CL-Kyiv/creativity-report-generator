using System;
using System.Collections.Generic;
using System.Text;

namespace CreativityReportGenerator.Services.Abstractions
{
    public interface IAppSettingsService
    {
        void ChangeCurrentService(string currentService);
    }
}
