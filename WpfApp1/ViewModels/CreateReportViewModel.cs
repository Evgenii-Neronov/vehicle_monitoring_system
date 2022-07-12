using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using ReportsLib;
using ReportsLib.Models;
using ReportsLib.Models.ReportServer;
using WpfApp1.Models;
using WpfApp1.Models.Validators;
using WpfApp1.Models.Validators.Rules;
using WpfApp1.Services;
using Action = System.Action;
using ValidationResult = WpfApp1.Models.Validators.ValidationResult;

namespace WpfApp1.ViewModels
{
    public class CreateReportViewModel : ViewModelBase
    {
        private IReportsDataAccess _reportApiService;
        private IValidator _validator;
        
        public CreateReportViewModel()
        {
            this._validator = new CreateReportValidator();
            InitAsync();
        }
        
        public async Task InitAsync()
        {
            await Task.Factory.StartNew(async () =>
            {
                try
                {

                    this._reportApiService = ReportsApiClientSingleton.Instance;

                    this.InitConnectionEvents(this._reportApiService);

                    this._firstReportDateTime = DateTime.Now;

                    var reportTypes = await this._reportApiService.GetReportTypesAsync();
                    var monitoringObjects = await this._reportApiService.GetMonitoringObjectsAsync();
                    var reportPeriodicity = await this._reportApiService.GetReportPeriodicityDefinesAsync();

                    this.ReportTypes = new BindableCollection<ReportType>(reportTypes);
                    this.MonitoringObjects = new BindableCollection<MonitoringObject>(monitoringObjects);
                    this.ReportPeriodicityDefines = new BindableCollection<ReportPeriodicityDefine>(reportPeriodicity);
                    this.BuildingParameters = new BindableCollection<BuildingParameter>();

                    InitValidationRules();

                    OnPropertyChanged(nameof(this.MonitoringObjects));
                    OnPropertyChanged(nameof(this.ReportPeriodicityDefines));
                    OnPropertyChanged(nameof(this.BuildingParameters));
                    OnPropertyChanged(nameof(this.ReportTypes));
                }
                catch (ServerException e)
                {
                    this.ServerError = e.Message;
                    OnPropertyChanged(nameof(this.DisplayError));
                }
            });
        }

        /// <summary>
        /// Exclusive validation rule for building parameters
        /// </summary>
        private readonly BuildingParametersRule _buildingParametersRule = new();

        private void InitValidationRules()
        {
            var rules = new List<Tuple<string, IValidationRule>>()
            {
                new(nameof(this.JobName), new JobNameValidationRule()),
                new(nameof(this.Hours), new HoursValidationRule()),
                new(nameof(this.Minutes), new MinutesValidationRule()),
                new(nameof(this.ReportType), new ShouldNotBeNullRule("Report type")),
                new(nameof(this.MonitoringObject), new ShouldNotBeNullRule("Monitoring objects")),
                new(nameof(this.ReportPeriodicityDefine), new ShouldNotBeNullRule("Report periodicity")),
                new(nameof(this.SelectedBuildingParameters), _buildingParametersRule),
            };

            rules.ForEach(x=> this._validator.AddValidationRule(x.Item1, x.Item2));

        }

        private void Validate(string propertyName, object value)
        {
            ClearErrorInfoBlock();

            var validationResult = this._validator.Validate(propertyName, value);
            OnPropertyChanged(nameof(this.DisplayError));
            if (validationResult.IsSuccess == false)
            {
                throw new ValidationException(validationResult.Error);
            }
        }

        public bool ValidateAll()
        {
            var allValidations = new List<Func<ValidationResult>>()
            {
                () => this._validator.Validate(nameof(this.JobName), this.JobName),
                () => this._validator.Validate(nameof(this.ReportType), this.ReportType),
                () => this._validator.Validate(nameof(this.MonitoringObject), this.MonitoringObject),
                () => this._validator.Validate(nameof(this.Hours), this.Hours),
                () => this._validator.Validate(nameof(this.Minutes), this.Minutes),
                () => this._validator.Validate(nameof(this.ReportPeriodicityDefine), this.ReportPeriodicityDefine),
                () => this._validator.Validate(nameof(this.SelectedBuildingParameters), this.SelectedBuildingParameters),

            };

            var result = allValidations.Select(x => x().IsSuccess).All(x => x);

            OnPropertyChanged(nameof(this.DisplayError));

            return result;
        }

        public async Task CreateReport()
        {
            await Task.Factory.StartNew(async () =>
            {
                try
                {
                    var result = await this._reportApiService.CreateReportAsync(new CreateReportRequest()
                    {
                        ReportModel = new ReportModel()
                        {
                            ReportId = Guid.NewGuid(),
                            JobName = this.JobName,
                            FirstReportDateTime = this.FirstReportDateTime,
                            MonitoringObjects = this.SelectedMonitoringObjects.Select(x => x.Code).ToList(),
                            ReportPeriodicityId = this.ReportPeriodicityDefine.Id,
                            ReportTypeCode = this.ReportType.Code,
                            ReportBuildingParametersIds = this.SelectedBuildingParameters.Select(x => x.Id).ToList()
                        }
                    });

                    if (result.Success == false)
                        this.ServerError = result.Error;
                }
                catch (Exception e)
                {
                    this.ServerError = $"Can't create report: {e.Message}";
                }

                OnPropertyChanged(nameof(this.DisplayError));
            });
        }
        
        public string ServerError { get; set; }

        public void ClearErrorInfoBlock()
        {
            this.ServerError = string.Empty;
            OnPropertyChanged(nameof(this.DisplayError));
        }

        public string DisplayError
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.ServerError) == false)
                {
                    return this.ServerError;
                }

                var validationErrors = _validator.GetValidationErrors();
                return validationErrors.Any() 
                    ? validationErrors.FirstOrDefault().Value 
                    : string.Empty;
            }
        }

        public async Task UpdateBuildingParameters()
        {
            await Task.Factory.StartNew(async () =>
            {
                ArgumentNullException.ThrowIfNull(_reportType);
                try
                {
                    var buildingParametersDefine = await this._reportApiService.GetBuildingParametersDefineAsync(_reportType.Code);

                    this.BuildingParameters = new BindableCollection<BuildingParameter>();
                    this.BuildingParameters.AddRange(buildingParametersDefine.BuildingParameters);
                    // update exclusive validation rule:
                    _buildingParametersRule.IsAllowMultiply = buildingParametersDefine.IsAllowMultiply;
                }
                catch (ServerException ex)
                {
                    this.ServerError = ex.Message;
                }

                OnPropertyChanged(nameof(this.BuildingParameters));
            });
        }

        /// <summary>
        /// Report types
        /// </summary>
        public BindableCollection<ReportType> ReportTypes { get; set; }

        /// <summary>
        /// Monitoring objects
        /// </summary>
        public BindableCollection<MonitoringObject> MonitoringObjects { get; set; }

        /// <summary>
        /// Report periodicity defines
        /// </summary>
        public BindableCollection<ReportPeriodicityDefine> ReportPeriodicityDefines { get; set; }
        
        /// <summary>
        /// Building parameters
        /// </summary>
        public BindableCollection<BuildingParameter> BuildingParameters { get; set; }

        /// <summary>
        /// Is allow multiply building parameters
        /// </summary>
        public bool IsBuildingParametersAllowMultiply { get; set; }
        
        /// <summary>
        /// Job Description
        /// </summary>
        private string _jobName;

        public string JobName
        {
            get => _jobName;
            set
            {
                this.Validate(nameof(this.JobName), value);

                _jobName = value;
                OnPropertyChanged(nameof(JobName));

                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Fill job name");
                }
            }
        }

        /// <summary>
        /// Report Type
        /// </summary>
        private ReportType _reportType;

        public ReportType ReportType
        {
            get => _reportType;
            set
            {
                _reportType = value;
                this.UpdateBuildingParameters();
                OnPropertyChanged(nameof(ReportType));
            }
        }

        /// <summary>
        /// Monitoring Object
        /// </summary>
        private MonitoringObject _monitoringObject;

        public MonitoringObject MonitoringObject
        {
            get => _monitoringObject;
            set
            {
                this.Validate(nameof(this.MonitoringObject), value);
                _monitoringObject = value;
                OnPropertyChanged(nameof(MonitoringObject));
            }
        }

        /// <summary>
        /// Date time first report
        /// </summary>
        private DateTime _firstReportDateTime { get; set; }

        public DateTime FirstReportDateTime
        {
            get => _firstReportDateTime;
            set
            {
                _firstReportDateTime = value;
                OnPropertyChanged(nameof(this.FirstReportDateTime));
            }
        }

        private string _hours;

        public string Hours
        {
            get => _hours;
            set
            {
                _hours = value;

                var parseResult = int.TryParse(value, out var hours);
                this.Validate(nameof(this.Hours), parseResult ? hours : value);

                FirstReportDateTime = new DateTime(_firstReportDateTime.Year,
                                               _firstReportDateTime.Month,
                                               _firstReportDateTime.Day,
                                               hours,
                                               _firstReportDateTime.Minute,
                                               0);
            }
        }

        private string _minutes;

        public string Minutes
        {
            get => _minutes;
            set
            {
                this._minutes = value;

                var parseResult = int.TryParse(value, out var minutes);
                this.Validate(nameof(this.Minutes), parseResult ? minutes : value);

                FirstReportDateTime = new DateTime(_firstReportDateTime.Year,
                    _firstReportDateTime.Month,
                    _firstReportDateTime.Day,
                    _firstReportDateTime.Hour,
                    minutes,
                    0);
            }
        }

        /// <summary>
        /// Report periodicity
        /// </summary>
        private ReportPeriodicityDefine _reportPeriodicityDefine;

        public ReportPeriodicityDefine ReportPeriodicityDefine
        {
            get => _reportPeriodicityDefine;
            set
            {
                this.Validate(nameof(this.ReportPeriodicityDefine), value);
                OnPropertyChanged(nameof(ReportPeriodicityDefine));
                _reportPeriodicityDefine = value;
            }
        }

        /// <summary>
        /// Selected building parameters
        /// </summary>

        private List<MonitoringObject> _selectedMonitoringObjects;


        public List<MonitoringObject> SelectedMonitoringObjects
        {
            get => _selectedMonitoringObjects;
            set
            {
                _selectedMonitoringObjects = value;
                OnPropertyChanged(nameof(this.SelectedMonitoringObjects));
            }
        }

        /// <summary>
        /// Selected building parameters
        /// </summary>

        private List<BuildingParameter> _selectedBuildingParameters;


        public List<BuildingParameter> SelectedBuildingParameters
        {
            get => _selectedBuildingParameters;
            set
            {
                _selectedBuildingParameters = value;
                OnPropertyChanged(nameof(this.SelectedBuildingParameters));
            }
        }
    }
}
