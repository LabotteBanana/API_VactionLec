using System;

namespace DotnetCoreServer.Models
{
    // result model : API 응답 모델의 기본 클래스
    public class LoginResult : ResultBase{
        public User Data;

    }
}