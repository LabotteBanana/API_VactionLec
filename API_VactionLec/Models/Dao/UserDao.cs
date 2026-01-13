using MySqlConnector;

using System;
using System.Collections.Generic;

namespace DotnetCoreServer.Models
{
    public interface IUserDao{
        User FindUserByFUID(string FacebookID);
        User GetUser(long UserID);
        User InsertUser(User user);
        bool UpdateUser(User user);
    }

    public class UserDao : IUserDao
    {
        public IDB db {get;}

        public UserDao(IDB db){
            this.db = db;
        }
        // FacebookID로 유저 찾기, Post /Login/Facebook일때 사용
        public User FindUserByFUID(string FacebookID){
            User user = new User();                             // 1. User 객체 생성
            using(MySqlConnection conn = db.GetConnection())    // 2. DB 연결 요청
            {   
                string query = String.Format(   // 3. SQL 쿼리문 작성, Facebook_id로 유저 검색
                    "SELECT user_id, facebook_id, facebook_name, facebook_photo_url, point, created_at, access_token FROM tb_user WHERE facebook_id = '{0}'",
                     FacebookID);

                Console.WriteLine(query);

                using(MySqlCommand cmd = (MySqlCommand)conn.CreateCommand())    
                {
                    cmd.CommandText = query;
                    using (MySqlDataReader reader = (MySqlDataReader)cmd.ExecuteReader()) // 4. 커맨드 객체 생성, MySql로 SQL 전송
                    {
                        if (reader.Read())  // 5. 결과값(reader)이 있으면 User 객체에 데이터 매핑
                        {
                            user.UserID = reader.GetInt64(0);
                            user.FacebookID = reader.GetString(1);
                            user.FacebookName = reader.GetString(2);
                            user.FacebookPhotoURL = reader.GetString(3);
                            user.Point = reader.GetInt32(4);
                            user.CreatedAt = reader.GetDateTime(5);
                            user.AccessToken = reader.GetString(6);
                            return user;    // 6. User 결과 객체 반환
                        }
                    }
                }
                conn.Close();
            }
            return null;
        }

        // UserID로 유저 찾기, GET /Login/{id}일때 사용
        public User GetUser(long UserID){
            User user = new User();
            using(MySqlConnection conn = db.GetConnection())
            {   
                string query = String.Format(
                    @"
                    SELECT 
                        user_id, facebook_id, facebook_name, 
                        facebook_photo_url, point, created_at, 
                        access_token, diamond, health, defense, damage,
                        speed, health_level, defense_level, 
                        damage_level, speed_level,
                        level, experience
                    FROM tb_user 
                    WHERE user_id = {0}",
                     UserID);

                Console.WriteLine(query);

                using(MySqlCommand cmd = (MySqlCommand)conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    using (MySqlDataReader reader = (MySqlDataReader)cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user.UserID = reader.GetInt64(0);
                            user.FacebookID = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            user.FacebookName = reader.IsDBNull(2) ? "" : reader.GetString(2);
                            user.FacebookPhotoURL = reader.IsDBNull(3) ? "" : reader.GetString(3);
                            user.Point = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
                            user.CreatedAt = reader.GetDateTime(5);
                            user.AccessToken = reader.IsDBNull(6) ? "" : reader.GetString(6);

                            user.Diamond = reader.IsDBNull(7) ? 0 : reader.GetInt32(7);
                            user.Health = reader.IsDBNull(8) ? 0 : reader.GetInt32(8);
                            user.Defense = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);
                            user.Damage = reader.IsDBNull(10) ? 0 : reader.GetInt32(10);
                            user.Speed = reader.IsDBNull(11) ? 0 : reader.GetInt32(11);

                            user.HealthLevel = reader.IsDBNull(12) ? 0 : reader.GetInt32(12);
                            user.DefenseLevel = reader.IsDBNull(13) ? 0 : reader.GetInt32(13);
                            user.DamageLevel = reader.IsDBNull(14) ? 0 : reader.GetInt32(14);
                            user.SpeedLevel = reader.IsDBNull(15) ? 0 : reader.GetInt32(15);

                            user.Level = reader.IsDBNull(16) ? 1 : reader.GetInt32(16);
                            user.Experience = reader.IsDBNull(17) ? 0 : reader.GetInt32(17);

                        }
                    }
                }
                
                conn.Close();
            }
            return user;
        }

        public User InsertUser(User user){
            
            string query = String.Format(
                "INSERT INTO tb_user (facebook_id, facebook_name, facebook_photo_url, point, access_token, created_at) VALUES ('{0}','{1}','{2}',{3}, '{4}', now())",
                    user.FacebookID, user.FacebookName, user.FacebookPhotoURL, 0, user.AccessToken);

            Console.WriteLine(query);

            using(MySqlConnection conn = db.GetConnection())
            using(MySqlCommand cmd = (MySqlCommand)conn.CreateCommand())
            {

                cmd.CommandText = query;
                cmd.ExecuteNonQuery();

                conn.Close();
            }

        
            return user;
        }

        public bool UpdateUser(User user){
            using(MySqlConnection conn = db.GetConnection())
            {
                string query = String.Format(
                    @"
                    UPDATE tb_user SET 
                        health = {0}, defense = {1}, damage = {2}, speed = {3},
                        health_level = {4}, defense_level = {5}, damage_level = {6}, speed_level = {7},
                        diamond = {8}, point = {9}
                    WHERE user_id = {10}
                    ",
                    user.Health, user.Defense, user.Damage, user.Speed,
                    user.HealthLevel, user.DefenseLevel, user.DamageLevel, user.SpeedLevel,
                    user.Diamond, user.Point, user.UserID
                     );

                Console.WriteLine(query);
                
                using(MySqlCommand cmd = (MySqlCommand)conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    
                }

                conn.Close();
            }
            return true;
        }



    }
}