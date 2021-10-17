using OnlineSchool.Contract.Courses;
using OnlineSchool.Contract.Resources;
using OnlineSchool.Contract.SpotMeeting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSchool.Core.Helper_
{
    public static class GridHelper
    {
        public static string ConfigureSpotMeetingActions(SpotMeetingModel data, bool isAdmin)
        {
            var sb = new StringBuilder();

            sb.Append($"<button class='btn btn-sm btn-light border m-1' title='{SharedResources.CheckProgram}' onclick='onCheckProgram({data.Id})'> ");
            sb.Append("<i class='fas fa-clipboard-list'></i> ");
            sb.Append("</button> ");

            switch (data.Status)
            {
                case SpotMeetingStatus.NotPublished:
                    sb.Append($"<button class='btn btn-sm btn-light border m-1' title='{SharedResources.Edit}' onclick='onEditSpotMeeting({data.Id})'> ");
                    sb.Append("<i class='fas fa-edit'></i> ");
                    sb.Append("</button> ");
                    sb.Append($"<button class='btn btn-sm btn-primary m-1' title='{SharedResources.Publish}' onclick='onPublishSpotMeeting({data.Id})'> ");
                    sb.Append("<i class='fa fa-bullhorn'></i> ");
                    sb.Append("</button> ");
                    if (isAdmin)
                    {
                        sb.Append($"<button class='btn btn-sm btn-danger text-white m-1' title='{SharedResources.Delete}' onclick='onDeleteSpotMeeting({data.Id})'> ");
                        sb.Append("<i class='far fa-trash-alt'></i> ");
                        sb.Append("</button> ");
                    }
                    break;
                case SpotMeetingStatus.Published:
                    sb.Append($"<button class='btn btn-sm btn-light border m-1' title='{SharedResources.Edit}' onclick='onEditSpotMeeting({data.Id})'> ");
                    sb.Append("<i class='fas fa-edit'></i> ");
                    sb.Append("</button> ");
                    break;
                default:
                    break;
            }

            return sb.ToString();
        }

        public static string ConfigureCourseActions(CourseModel data, bool isAdmin)
        {
            var sb = new StringBuilder();

            switch (data.Status)
            {
                case CourseStatus.NotPublished:
                    sb.Append($"<button class='btn btn-sm btn-light border m-1' title='{SharedResources.Edit}' onclick='onEditCourse({data.Id})'> ");
                    sb.Append("<i class='fas fa-edit'></i> ");
                    sb.Append("</button> ");
                    sb.Append($"<button class='btn btn-sm btn-primary m-1' title='{SharedResources.Publish}' onclick='onPublishCourse({data.Id})'> ");
                    sb.Append("<i class='fa fa-bullhorn'></i> ");
                    sb.Append("</button> ");
                    if (isAdmin)
                    {
                        sb.Append($"<button class='btn btn-sm btn-danger text-white m-1' title='{SharedResources.Delete}' onclick='onDeleteCourse({data.Id})'> ");
                        sb.Append("<i class='far fa-trash-alt'></i> ");
                        sb.Append("</button> ");
                    }
                    break;
                case CourseStatus.Published:
                    sb.Append($"<button class='btn btn-sm btn-light border m-1' title='{SharedResources.Edit}' onclick='onEditCourse({data.Id})'> ");
                    sb.Append("<i class='fas fa-edit'></i> ");
                    sb.Append("</button> ");
                    break;
                default:
                    break;
            }

            return sb.ToString();
        }
    }
}
