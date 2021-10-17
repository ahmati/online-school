using ItalWebConsulting.Infrastructure.BusinessLogic;
using ItalWebConsulting.Infrastructure.Comunication;
using ItalWebConsulting.Infrastructure.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OnlineSchool.Contract.Calendar;
using OnlineSchool.Contract.Contacts;
using OnlineSchool.Contract.Infrastructure;
using OnlineSchool.Contract.Payment;
using OnlineSchool.Contract.SpotMeeting;
using OnlineSchool.Contract.Webex;
using OnlineSchool.Core.Helper_;
using OnlineSchool.Core.IcsCalendar.Service;
using OnlineSchool.Core.Identity_.Services;
using OnlineSchool.Core.Infrastructure.FileUpload;
using OnlineSchool.Core.Payment_.Service;
using OnlineSchool.Core.SpotMeeting_.Repository;
using OnlineSchool.Core.Teachers_.Service;
using OnlineSchool.Core.Webex_.Services;
using OnlineSchool.Domain.OnlineSchoolDB.EF;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSchool.Core.SpotMeeting_.Service
{
    public class SpotMeetingService : CoreBase, ISpotMeetingService
    {
        public ISpotMeetingRepository SpotMeetingRepository { get; set; }
        public IFileService FileService { get; set; }
        public IIdentityService IdentityService { get; set; }
        public IEmailService EmailService { get; set; }
        public IPaymentService PaymentService { get; set; }
        public IWebexService WebexService { get; set; }
        public ITeacherService TeacherService { get; set; }
        public IIcsCalendarService CalendarService { get; set; }

        public async Task<SpotMeetingModel> GetByIdAsync(int id)
        {
            try
            {
                var data = await SpotMeetingRepository.GetByIdAsync(id);
                data.StartDate = data.StartDate.ConvertTimeFromUtc();

                var result = Mapper.Map<SpotMeetingModel>(data);

                var spotMeetingTeacher = await SpotMeetingRepository.GetHostTeacherAsync(id);
                result.Host = Mapper.Map<SpotMeetingTeacherModel>(spotMeetingTeacher);

                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                throw ex;
            }
        }

        public async Task<IEnumerable<SpotMeetingModel>> GetAllAsync()
        {
            try
            {
                var data = await SpotMeetingRepository.GetAllAsync();
                var result = Mapper.Map<IEnumerable<SpotMeetingModel>>(data);
                foreach (var item in result)
                {
                    item.Buttons = GridHelper.ConfigureSpotMeetingActions(item, true);

                    var spotMeetingTeacher = await SpotMeetingRepository.GetHostTeacherAsync(item.Id);
                    item.Host = Mapper.Map<SpotMeetingTeacherModel>(spotMeetingTeacher);
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError("Errore di imprevisto.", ex.Message);
                throw ex;
            }
        }

        public async Task<IEnumerable<SpotMeetingModel>> GetAllPublishedAsync()
        {
            try
            {
                var data = await SpotMeetingRepository.GetAllPublishedAsync();
                
                var result = Mapper.Map<IEnumerable<SpotMeetingModel>>(data);
                result = result.Where(r => r.Status == SpotMeetingStatus.Published || r.Status == SpotMeetingStatus.InProgress);

                foreach (var item in result)
                {
                    var isOngoing = await WebexService.CheckHasMeetingStartedAsync(item.MeetingId);
                    item.IsLive = isOngoing.Output;
                }

                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError("Errore di imprevisto.", ex.Message);
                throw ex;
            }
        }

        public async Task<IEnumerable<SpotMeetingModel>> GetAllRecursiveAsync()
        {
            try
            {
                var data = await SpotMeetingRepository.GetAllRecursiveAsync();
                return Mapper.Map<IEnumerable<SpotMeetingModel>>(data);
            }
            catch (Exception ex)
            {
                Logger.LogError("Errore di imprevisto.", ex.Message);
                throw ex;
            }
        }

        public async Task<IEnumerable<SpotMeetingModel>> GetSpotMeetingsThatNeedRemindingAsync()
        {
            try
            {
                var data = await SpotMeetingRepository.GetSpotMeetingsThatNeedRemindingAsync();
                return Mapper.Map<IEnumerable<SpotMeetingModel>>(data);
            }
            catch (Exception ex)
            {
                Logger.LogError("Errore di imprevisto.", ex.Message);
                throw ex;
            }
        }

        public async Task<IEnumerable<BookedSpotMeetingModel>> GetUsersBySpotMeetingAsync(int spotMeetingId)
        {
            try
            {
                var data = await SpotMeetingRepository.GetUsersBySpotMeetingAsync(spotMeetingId);
                return Mapper.Map<IEnumerable<BookedSpotMeetingModel>>(data);
            }
            catch (Exception ex)
            {
                Logger.LogError("Errore di imprevisto.", ex.Message);
                throw ex;
            }
        }

        public async Task<IEnumerable<BookedSpotMeetingModel>> GetBookedSpotMeetingsBySpotMeetingIdAsync(int id)
        {
            try
            {
                var data = await SpotMeetingRepository.GetBookedSpotMeetingsBySpotMeetingIdAsync(id);

                return Mapper.Map<IEnumerable<BookedSpotMeetingModel>>(data);
            }
            catch (Exception ex)
            {
                Logger.LogError("Errore di imprevisto.", ex.Message);
                throw ex;
            }
        }

        public async Task<ResponseBase<CreateSpotMeetingModel>> CreateAsync(CreateSpotMeetingModel model)
        {
            /* Workflow
             * 1. Validate the model
             * 2. If model is valid, create the Webex Meeting. If this fails, don't continue.
             * 3. If Webex Meeting is created successfully, add Spot Meeting to db with the required data
            */
            var output = new ResponseBase<CreateSpotMeetingModel>();
            try
            {
                if (model is null) 
                    throw new ArgumentNullException(nameof(model));

                var validation = CreateMeetingValidation(model);
                if (validation.HasErrors)
                {
                    output.AddErrors(validation.Errors);
                    return output;
                }

                var createMeetingModel = new CreateMeetingModel()
                {
                    title = $"SpotMeeting-{model.Title}_{Guid.NewGuid()}",
                    start = model.StartDate.Date.Add(model.StartTime).ToString("s", CultureInfo.InvariantCulture),
                    end = model.StartDate.Date.Add(model.StartTime.Add(TimeSpan.FromHours(model.Duration))).ToString("s", CultureInfo.InvariantCulture)
                };

                var meeting = await WebexService.CreateMeetingAsync(createMeetingModel);
                if (meeting.HasErrors)
                {
                    output.AddErrors(meeting.Errors);
                    return output;
                }

                var spotMeeting = Mapper.Map<SpotMeeting>(model);
                spotMeeting.IsPublished = false;
                spotMeeting.StartDate = model.StartDate.Add(model.StartTime);
                spotMeeting.StartDate = spotMeeting.StartDate.ConvertTimeToUtc();
                spotMeeting.MeetingId = meeting.Output.id;
                spotMeeting.MeetingTitle = meeting.Output.title;
                spotMeeting.MeetingPassword = meeting.Output.password;
                spotMeeting.MeetingSipAddress = meeting.Output.sipAddress;

                spotMeeting = await SpotMeetingRepository.CreateAsync(spotMeeting);

                var fileResult = await FileService.UploadSingleAsync(model.ImageFile, $"wwwroot\\uploads\\spotMeetings\\{spotMeeting.Id}");

                var teacher = await TeacherService.GetAllTeacherById(model.HostId);

                var spotMeetingTeacher = new SpotMeetingTeacherModel();
                spotMeetingTeacher.SpotMeetingId = spotMeeting.Id;
                spotMeetingTeacher.TeacherId = model.HostId;
                spotMeetingTeacher.IsHost = true;

                await CreateSpotMeetingTeacherAsync(spotMeetingTeacher);

                output.Output = Mapper.Map<CreateSpotMeetingModel>(spotMeeting);
                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred.", ex.Message);
                output.AddError("An error occurred. The meeting could not be created.");
                return output;
            }

        }

        public async Task<ResponseBase<bool>> UpdateReminderEmailAsync(int spotMeetingId)
        {
            var output = new ResponseBase<bool>();
            try
            {
                var result = await SpotMeetingRepository.UpdateReminderEmailAsync(spotMeetingId);

                output.Output = result;
                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                output.AddError(ex.Message);
                return output;
            }

        }

        public async Task<ResponseBase<BookedSpotMeetingModel>> CreateBookedSpotMeetingAsync(BookedSpotMeetingModel model)
        {
            var output = new ResponseBase<BookedSpotMeetingModel>();
            try
            {
                var bookedSpotMeeting = Mapper.Map<BookedSpotMeeting>(model);

                bookedSpotMeeting = await SpotMeetingRepository.CreateBookedSpotMeetingAsync(bookedSpotMeeting);
                
                output.Output = Mapper.Map<BookedSpotMeetingModel>(bookedSpotMeeting);
                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred.", ex.Message);
                return output;
            }
        }

        public async Task<ResponseBase<bool>> UpdateAsync(UpdateSpotMeetingModel model)
        {
            var output = new ResponseBase<bool>();
            try
            {
                var spotMeeting = await SpotMeetingRepository.GetByIdAsync(model.Id);
                if (spotMeeting is null)
                {
                    output.AddError("An error occurred. The meeting was not updated.");
                    return output;
                }

                spotMeeting.Title = model.Title;
                spotMeeting.Description = model.Description;
                spotMeeting.Duration = model.Duration;
                spotMeeting.StartDate = model.StartDate.Add(model.StartTime);
                spotMeeting.StartDate = spotMeeting.StartDate.ConvertTimeToUtc();
                spotMeeting.Price = model.Price;
                spotMeeting.IsRecursiveSpotMeeting = model.IsRecursiveSpotMeeting;
                if (model.ImageFile != null)
                    spotMeeting.ImagePath = model.ImageFile.FileName;

                // Gets current host
                var spotMeetingHostTeacher = await SpotMeetingRepository.GetHostTeacherAsync(spotMeeting.Id);

                // If choosen host is different from current host
                if (model.HostId != spotMeetingHostTeacher?.TeacherId)
                {
                    // Unassing teacher from host 
                    spotMeetingHostTeacher.IsHost = false;
                    var result = await SpotMeetingRepository.UpdateSpotMeetingTeacherAsync(spotMeetingHostTeacher);
                    if (!result)
                    {
                        output.AddError("An error occurred. The meeting was not updated.");
                        return output;
                    }

                    // Check if choosen teacher is a teacher of the spot meeting
                    var spotMeetingTeacher = await SpotMeetingRepository.GetSpotMeetingTeacherByIdAsync(model.HostId);
                    if (spotMeetingTeacher != null)
                    {
                        // If yes assigns teacher as host
                        spotMeetingTeacher.IsHost = true;
                        var result2 = await SpotMeetingRepository.UpdateSpotMeetingTeacherAsync(spotMeetingTeacher);
                        if (!result2)
                        {
                            output.AddError("An error occurred. The meeting was not updated.");
                            return output;
                        }
                    }
                    else
                    {
                        // If not creates a new SpotMeetingTeacher and assigns as host
                        var host = new SpotMeetingTeacherModel
                        {
                            SpotMeetingId = spotMeeting.Id,
                            TeacherId = model.HostId,
                            IsHost = true
                        };

                        var result2 = await CreateSpotMeetingTeacherAsync(host);
                        if (result2.HasErrors)
                        {
                            output.AddErrors(result2.Errors);
                            return output;
                        }
                    }
                }

                var updated = await SpotMeetingRepository.UpdateAsync(Mapper.Map<SpotMeeting>(spotMeeting));
                if (!updated)
                {
                    output.AddError("An error occurred. The meeting was not updated.");
                    return output;
                }

                var fileResult = await ReplaceImageAsync(model.ImageFile, spotMeeting.Id);

                output.Output = true;
                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred.", ex.Message);
                return output;
            }
        }

        public async Task<ResponseBase<bool>> RenewSpotMeetingAsync(UpdateSpotMeetingModel model)
        {
            var output = new ResponseBase<bool>();
            try
            {
                var spotMeeting = await SpotMeetingRepository.GetByIdAsync(model.Id);
                if (spotMeeting is null)
                {
                    output.AddError("An error occurred. The meeting was not updated.");
                    return output;
                }

                spotMeeting.StartDate = model.StartDate;
                var updated = await SpotMeetingRepository.UpdateAsync(Mapper.Map<SpotMeeting>(spotMeeting));
                if (!updated)
                {
                    output.AddError("An error occurred. The meeting was not updated.");
                    return output;
                }

                output.Output = true;
                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred.", ex.Message);
                return output;
            }
        }

        public async Task<ResponseBase<bool>> DeleteAsync(int id)
        {
            var output = new ResponseBase<bool>();
            var errorMessage = "An error occurred. Meeting could not be deleted.";
            try
            {
                var spotMeeting = await GetByIdAsync(id);
                if (spotMeeting is null)
                {
                    output.AddError(errorMessage);
                    return output;
                }

                var deleted = await SpotMeetingRepository.DeleteAsync(id);
                if (!deleted)
                {
                    output.AddError(errorMessage);
                    return output;
                }

                output.Output = true;
                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred.", ex.Message);
                output.AddError(errorMessage);
                return output;
            }
        }

        public async Task<ResponseBase<bool>> PublishAsync(int id)
        {
            var output = new ResponseBase<bool>();
            try
            {
                var spotMeeting = await SpotMeetingRepository.GetByIdAsync(id);
                if (spotMeeting is null)
                {
                    output.AddError("The meeting could not be found.");
                    return output;
                }

                spotMeeting.IsPublished = true;
                var updated = await SpotMeetingRepository.UpdateAsync(spotMeeting);
                if (!updated)
                {
                    output.AddError("An error occurred. Meeting could not be published.");
                    return output;
                }

                output.Output = true;
                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred.", ex.Message);
                return output;
            }
        }

        public async Task<ResponseBase<bool>> UnpublishAsync(int id)
        {
            var output = new ResponseBase<bool>();
            try
            {
                var spotMeeting = await SpotMeetingRepository.GetByIdAsync(id);
                if (spotMeeting is null)
                {
                    output.AddError("The meeting could not be found.");
                    return output;
                }

                spotMeeting.IsPublished = false;
                var updated = await SpotMeetingRepository.UpdateAsync(spotMeeting);
                if (!updated)
                {
                    output.AddError("An error occurred. Meeting could not be unpublished.");
                    return output;
                }

                output.Output = true;
                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred.", ex.Message);
                return output;
            }
        }

        public async Task<ResponseBase<bool>> ReplaceImageAsync(IFormFile file, int spotMeetingId)
        {
            var output = new ResponseBase<bool>();

            if (file is null)
                return output;

            if (spotMeetingId < 1)
                throw new ArgumentException("Invalid meeting id");

            try
            {
                var destination = $"wwwroot\\uploads\\spotMeetings\\{spotMeetingId}";

                if (Directory.Exists(destination))
                {
                    var existingFiles = Directory.GetFiles(destination);
                    foreach (var f in existingFiles)
                        File.Delete(f);
                }

                var result = await FileService.UploadSingleAsync(file, destination);
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                output.AddError(ex.Message);
                return output;
            }
        }

        #region ----------------------------------------- Material Actions -----------------------------------------
        public async Task<IEnumerable<SpotMeetingMaterialModel>> GetMaterialsAsync(int spotMeetingId)
        {
            try
            {
                var data = await SpotMeetingRepository.GetMaterialsAsync(spotMeetingId);
                return Mapper.Map<IEnumerable<SpotMeetingMaterialModel>>(data);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<ResponseBase<SpotMeetingMaterialModel>> GetSpotMeetingMaterialByIdAsync(int id)
        {
            var output = new ResponseBase<SpotMeetingMaterialModel>();

            try
            {
                var data = await SpotMeetingRepository.GetSpotMeetingMaterialByIdAsync(id);
                var result = Mapper.Map<SpotMeetingMaterialModel>(data);
                output.Output = result;
            }
            catch (Exception ex)
            {
                Logger.LogError("Error encountered.", ex.Message);
                output.AddError("An error occurred while retrieving material.");
            }
            return output;
        }

        public async Task<ResponseBase<IEnumerable<SpotMeetingMaterialModel>>> GetSpotMeetingMaterialByIdsAsync(int[] ids)
        {
            var output = new ResponseBase<IEnumerable<SpotMeetingMaterialModel>>();

            try
            {
                var data = await SpotMeetingRepository.GetSpotMeetingMaterialByIdsAsync(ids);
                var result = Mapper.Map<IEnumerable<SpotMeetingMaterialModel>>(data);
                output.Output = result;
            }
            catch (Exception ex)
            {
                Logger.LogError("Error encountered.", ex.Message);
                output.AddError("An error occurred while retrieving material.");
            }
            return output;
        }

        public async Task<ResponseBase<SpotMeetingMaterialModel>> GetSpotMeetingMaterialByFileNameAsync(int spotMeetingId, string fileName)
        {
            var output = new ResponseBase<SpotMeetingMaterialModel>();

            try
            {
                var data = await SpotMeetingRepository.GetSpotMeetingMaterialByFileNameAsync(spotMeetingId, fileName);
                var result = Mapper.Map<SpotMeetingMaterialModel>(data);
                output.Output = result;
            }
            catch (Exception ex)
            {
                Logger.LogError("Error encountered.", ex.Message);
                output.AddError("An error occurred while retrieving material.");
            }
            return output;
        }

        public async Task<ResponseBase<SpotMeetingMaterialModel>> CreateSpotMeetingMaterialAsync(SpotMeetingMaterialModel model)
        {
            var output = new ResponseBase<SpotMeetingMaterialModel>();

            try
            {
                var spotMeetingMaterial = Mapper.Map<SpotMeetingMaterial>(model);
                spotMeetingMaterial = await SpotMeetingRepository.CreateSpotMeetingMaterialAsync(spotMeetingMaterial);
                output.Output = Mapper.Map<SpotMeetingMaterialModel>(spotMeetingMaterial);

                return output;

            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                output.AddError(ex.Message);
                return output;
            }
        }

        public async Task<ResponseBase<bool>> DeleteSpotMeetingMaterialAsync(int id)
        {
            var output = new ResponseBase<bool>();
            try
            {
                var deleted = await SpotMeetingRepository.DeleteSpotMeetingMaterialAsync(id);
                if (!deleted)
                {
                    output.AddError("An error occurred. Material could not be deleted.");
                    return output;
                }

                output.Output = true;
                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred.", ex.Message);
                output.AddError(ex.Message);
                return output;
            }
        }
        #endregion

        #region ----------------------------------------- Teacher Actions -----------------------------------------
        public async Task<IEnumerable<SpotMeetingTeacherModel>> GetTeachersAsync(int spotMeetingId)
        {
            try
            {
                var data = await SpotMeetingRepository.GetTeachersAsync(spotMeetingId);
                return Mapper.Map<IEnumerable<SpotMeetingTeacherModel>>(data);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<IEnumerable<SpotMeetingModel>> GetTeacherSpotMeetingsAsync(string email)
        {
            try
            {
                var spotMeetingIds = await SpotMeetingRepository.GetSpotMeetingIdsByTeacherEmailAsync(email);
                var data = await SpotMeetingRepository.GetTeacherSpotMeetingsAsync(spotMeetingIds); 
                var result = Mapper.Map<IEnumerable<SpotMeetingModel>>(data);
                foreach (var item in result)
                {
                    item.Buttons = GridHelper.ConfigureSpotMeetingActions(item, false);

                    var spotMeetingTeacher = await SpotMeetingRepository.GetHostTeacherAsync(item.Id);
                    item.Host = Mapper.Map<SpotMeetingTeacherModel>(spotMeetingTeacher);
                }
                return result;
            }
            catch (Exception e)
            {
                Logger.LogError(e, "eErrore di imprevisto");
                return null;
            }
        }

        public async Task<SpotMeetingTeacherModel> GetSpotMeetingTeacherAsync(string email, int id)
        {
            try
            {
                var result = await SpotMeetingRepository.GetSpotMeetingTeacherAsync(email, id);
                return Mapper.Map<SpotMeetingTeacherModel>(result);
            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                return null;
            }
        }

        public async Task<SpotMeetingTeacherModel> GetSpotMeetingTeacherByIdAsync(int id)
        {
            try
            {
                var result = await SpotMeetingRepository.GetSpotMeetingTeacherByIdAsync(id);
                return Mapper.Map<SpotMeetingTeacherModel>(result);
            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                return null;
            }
        }

        public async Task<ResponseBase<SpotMeetingTeacherModel>> CreateSpotMeetingTeacherAsync(SpotMeetingTeacherModel model)
        {
            var output = new ResponseBase<SpotMeetingTeacherModel>();

            try
            {
                var exists = (await SpotMeetingRepository.GetSpotMeetingTeacherByIdsAsync(model.SpotMeetingId, model.TeacherId)) != null;
                if (exists)
                {
                    output.AddError($"This teacher is already assigned to this meeting.");
                    return output;
                }

                var spotMeetingTeacher = Mapper.Map<SpotMeetingTeacher>(model);
                spotMeetingTeacher = await SpotMeetingRepository.CreateSpotMeetingTeacherAsync(spotMeetingTeacher);
                output.Output = Mapper.Map<SpotMeetingTeacherModel>(spotMeetingTeacher);

                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                output.AddError(ex.Message);
                return output;
            }
        }

        public async Task<ResponseBase<bool>> DeleteSpotMeetingTeacherAsync(int id)
        {
            var output = new ResponseBase<bool>();
            try
            {
                var deleted = await SpotMeetingRepository.DeleteSpotMeetingTeacherAsync(id);
                if (!deleted)
                {
                    output.AddError("An error occurred. Teacher could not be deleted.");
                    return output;
                }

                output.Output = true;
                return output;
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred.", ex.Message);
                output.AddError(ex.Message);
                return output;
            }
        }
        #endregion

        #region ------------------------------------- PurchasableItem Methods -------------------------------------
        public async Task<PurchasableItemModel> GetPurchasableItemByIdAsync(int id)
        {
            try
            {
                var data = await GetByIdAsync(id);

                return data;
            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                throw ex;
            }
        }

        public async Task<ResponseBase<bool>> GetExistingPurchasedItemAsync(string email, int id)
        {
            var result = new ResponseBase<bool>();
            try
            {
                var data = await IdentityService.GetExistingBookedSpotMeetingAsync(email, id);
                if (data != null)
                {
                    result.Output = true;
                }

                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError("errore di imprevisto.", ex.Message);
                throw ex;
            }
        }

        public async Task<ResponseBase<bool>> BuyAsync(string stripeToken, int id, string email)
        {
            #region Validations
            if (string.IsNullOrWhiteSpace(stripeToken))
                throw new ArgumentNullException(nameof(stripeToken));

            var spotMeeting = await GetByIdAsync(id);
            if (spotMeeting == null)
                throw new ArgumentNullException(nameof(spotMeeting));

            var user = await IdentityService.FindByEmailAsync(email);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            #endregion

            var result = new ResponseBase<bool>();
            try
            {
                var result2 = await PaymentService.PayIt(stripeToken, (decimal)spotMeeting.Price, user.Name, email);
                string message;

                if (result2.Output)
                {
                    var model = new BookedSpotMeetingModel
                    {
                        SpotMeetingId = id,
                        UserId = user.Id,
                        Price = (double)spotMeeting.Price
                    };

                    await CreateBookedSpotMeetingAsync(model);

                    message = $"Hi {user.Name.ToUpper()} <br>" +
                        $"You have successfully purchased the {spotMeeting.Title} meeting for the price of {spotMeeting.Price}€ . <br>" +
                        $"The meeting will start on {spotMeeting.StartDate.Date}.";
                }
                else
                {
                    message = $"Hi {user.Name.ToUpper()} <br>" +
                       $"You were not able to buy the {spotMeeting.Title} meeting.";
                }

                await EmailService.SendEmailAsync(email, "Payment unsuccessful", message);

                result.Output = true;
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred.", ex.Message);
                return result;
            }

        }

        public async Task<ResponseBase<bool>> JoinAsync(int id, string email)
        {
            #region Validations
            var spotMeeting = await GetByIdAsync(id);
            if (spotMeeting == null)
                throw new ArgumentNullException(nameof(spotMeeting));

            var user = await IdentityService.FindByEmailAsync(email);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            #endregion
            var result = new ResponseBase<bool>();
            try
            {
                var model = new BookedSpotMeetingModel
                {
                    SpotMeetingId = id,
                    UserId = user.Id,
                    Price = (double)spotMeeting.Price
                };

                var result2 = await CreateBookedSpotMeetingAsync(model);

                var calendarModel = new IcsCalendarModel()
                {
                    Email = email,
                    UserName = user.Name,
                    Description = spotMeeting.Description,
                    StartDate = spotMeeting.StartDate,
                    StartTime = spotMeeting.StartTime,
                    Title = spotMeeting.Title,
                    Duration = spotMeeting.Duration
                };
                var emailSent = await CalendarService.SendIcsFile(calendarModel);
                
                result.Output = result.Output = (result2 != null);
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred.", ex.Message);
                return result;
            }
        }
        #endregion

        #region ----------------------------------------- Private Methods -----------------------------------------
        private ResponseBase<bool> CreateMeetingValidation(CreateSpotMeetingModel model)
        {
            var output = new ResponseBase<bool>();

            if (model is null)
            {
                output.AddError("Missing information.");
                return output;
            }

            if (string.IsNullOrEmpty(model.Title) || string.IsNullOrWhiteSpace(model.Title))
                output.AddError("Meeting title is required.");

            if (model.Title.Length > 1000)
                output.AddError("Title cannot be longer than 1000 characters.");

            var start = model.StartDate.Date;
            var today = DateTime.Now.Date;

            if (start < today)
                output.AddError("Start date cannot be before today.");

            if ((start - today).Days < 1)
                output.AddError("Start date should be at least 1 days after the date of creation.");

            if (model.Duration < 1 || model.Duration > 24)
                output.AddError("Meeting duration must be at least 1 hour and no longer than 24 hours.");

            if (model.Price < 0)
                output.AddError("Invalid price");

            return output;
        }
        #endregion
    }
}
