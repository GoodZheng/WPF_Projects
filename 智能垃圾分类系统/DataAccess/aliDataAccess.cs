using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
//using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using 智能垃圾分类系统.Common;
using 智能垃圾分类系统.DataAccess.DataEntity;
using 智能垃圾分类系统.Model;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using MySql.Data.MySqlClient;

namespace 智能垃圾分类系统.DataAccess
{
    public class aliDataAccess
    {
        private static aliDataAccess instance;
        private aliDataAccess() { }
        public static aliDataAccess GetInstance()
        {
            return instance ?? (instance = new aliDataAccess());
        }

        MySqlConnection conn;
        MySqlCommand comm;
        MySqlDataAdapter adapter;

        private void Dispose()
        {
            if (adapter != null)
            {
                adapter.Dispose();
                adapter = null;
            }
            if (comm != null)
            {
                comm.Dispose();
                comm = null;
            }
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
                conn = null;
            }
        }

        /// <summary>
        /// 连接初始化
        /// </summary>
        /// <returns></returns>
        private bool DBConnection()
        {
            //string connStr = ConfigurationManager.ConnectionStrings["db"].ConnectionString;

            var configBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var configSection = configBuilder.GetSection("db");
            string connStr = "server=rm-2zeclam5o7x8uz378lo.mysql.rds.aliyuncs.com;user id=peano;password=1622990174aA@;database=test001";//configSection["connectionString"] ?? null; //从配置文件中获取连接字符串
            //来自：https://debajmecrm.com/how-to-use-app-config-or-configuration-files-in-net-core-console-application/
            //https://www.bilibili.com/video/BV1LA41127rU/?spm_id_from=333.337.search-card.all.click&vd_source=92410f904c692e61c986cb81c7a59d15

            if (conn == null)
                conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                return true;
            }
            catch(Exception ex)
            {

                return false;
            }
        }

        public UserEntity CheckUserInfo(string userName, string pwd)
        {
            try
            {
                if (DBConnection())
                {
                    string userSql = "select * from test001.usersall where user_name=@user_name and password=@pwd";
                    MySqlCommand mySqlCommand = new MySqlCommand(userSql, conn);
                    mySqlCommand.Parameters.AddWithValue("@user_name", userName);
                    mySqlCommand.Parameters.AddWithValue("@pwd", MD5Provider.GetMD5String(pwd + "@" + userName));
                    adapter = new MySqlDataAdapter(mySqlCommand);
                    //adapter.SelectCommand.Parameters.Add(new MySqlParameter("@user_name", SqlDbType.VarChar) { Value = userName });
                    //adapter.SelectCommand.Parameters.Add(new MySqlParameter("@pwd", SqlDbType.VarChar) { Value = "password1"});// MD5Provider.GetMD5String(pwd + "@" + userName) });

                    DataTable table = new DataTable();
                    int count = adapter.Fill(table);

                    if (count <= 0)
                        throw new Exception("用户名或密码不正确！");

                    DataRow dr = table.Rows[0];//取出查询到的用户

                    UserEntity userInfo = new UserEntity();
                    userInfo.UserId = dr.Field<int>("user_id");
                    userInfo.UserName = dr.Field<string>("user_name");
                    userInfo.RealName = dr.Field<string>("real_name");
                    userInfo.Password = dr.Field<string>("password");
                    userInfo.Avatar = dr.Field<string>("avatar");
                    userInfo.Gender = dr.Field<Int32>("gender");
                    userInfo.Points = dr.Field<Int32>("points");
                    userInfo.UserPhone = dr.Field<string>("user_phone");
                    userInfo.Email = dr.Field<string>("user_email");

                    return userInfo;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Dispose();
            }

            return null;
        }


        public List<CourseSeriesModel> GetCoursePlayRecord()
        {
            try
            {
                List<CourseSeriesModel> cModelList = new List<CourseSeriesModel>();
                if (DBConnection())
                {
                    string userSql = @"select a.course_name,a.course_id,b.play_count,b.is_growing,b.growing_rate ,
                                    c.platform_name
                                    from courses a
                                    left join play_record b
                                    on a.course_id = b.course_id
                                    left join platforms c
                                    on b.platform_id = c.platform_id
                                    order by a.course_id,c.platform_id";
                    adapter = new MySqlDataAdapter(userSql, conn);

                    DataTable table = new DataTable();
                    int count = adapter.Fill(table);

                    string courseId = "";
                    CourseSeriesModel cModel = null;

                    foreach (DataRow dr in table.AsEnumerable())
                    {
                        string tempId = dr.Field<string>("course_id");
                        if (courseId != tempId)
                        {
                            courseId = tempId;
                            cModel = new CourseSeriesModel();
                            cModelList.Add(cModel);

                            cModel.CourseName = dr.Field<string>("course_name");
                            cModel.SeriesColection = new LiveCharts.SeriesCollection();
                            cModel.SeriesList = new System.Collections.ObjectModel.ObservableCollection<SeriesModel>();
                        }
                        if (cModel != null)
                        {
                            cModel.SeriesColection.Add(new PieSeries
                            {
                                Title = dr.Field<string>("platform_name"),
                                Values = new ChartValues<ObservableValue> { new ObservableValue((double)dr.Field<decimal>("play_count")) },
                                DataLabels = false
                            });

                            cModel.SeriesList.Add(new SeriesModel
                            {
                                SeriesName = dr.Field<string>("platform_name"),
                                CurrentValue = dr.Field<decimal>("play_count"),
                                IsGrowing = dr.Field<Int32>("is_growing") == 1,
                                ChangeRate = (int)dr.Field<decimal>("growing_rate")
                            });
                        }
                    }
                }
                return cModelList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Dispose();
            }
        }


        public List<string> GetTeachers()
        {
            try
            {
                List<string> result = new List<string>();
                if (this.DBConnection())
                {
                    string sql = "select real_name from users where is_teacher=1";
                    adapter = new MySqlDataAdapter(sql, conn);

                    DataTable table = new DataTable();
                    int count = adapter.Fill(table);
                    if (count > 0)
                    {
                        result = table.AsEnumerable().Select(c => c.Field<string>("real_name")).ToList();
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Dispose();
            }
        }

        public List<CourseModel> GetCourses()
        {
            try
            {
                List<CourseModel> result = new List<CourseModel>();
                if (this.DBConnection())
                {
                    string sql = @"select a.course_id,a.course_name,a.course_cover,a.course_url,a.description,c.real_name from courses a
left join course_teacher_relation b
on a.course_id=b.course_id
left join users c
on b.teacher_id=c.user_id
order by a.course_id";
                    adapter = new MySqlDataAdapter(sql, conn);

                    DataTable table = new DataTable();
                    int count = adapter.Fill(table); //填充
                    if (count > 0)
                    {
                        string courseId = "";
                        CourseModel model = null;

                        foreach (DataRow dr in table.AsEnumerable())
                        {
                            string tempId = dr.Field<string>("course_id");
                            if (courseId != tempId)
                            {
                                courseId = tempId;

                                model = new CourseModel();
                                model.CourseName = dr.Field<string>("course_name");
                                model.Cover = dr.Field<string>("course_cover");
                                model.Url = dr.Field<string>("course_url");
                                model.Description = dr.Field<string>("description");

                                model.Teachers = new List<string>();

                                result.Add(model);
                            }
                            if (model != null)
                            {
                                model.Teachers.Add(dr.Field<string>("real_name"));
                            }
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Dispose();
            }
        }
    }
}
