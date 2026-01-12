using System.Collections.Generic;

namespace DotnetCoreServer.Models{
    // Request Model : 클라이언트에서 서버로 요청할 때 사용하는 데이터 모델
    public class RankRequest
    {
        
        public long UserID;
        public List<string> FriendList;
    }
}