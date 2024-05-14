<<<<<<<< HEAD:Shared/Front/RequestDto.cs
﻿namespace Shared.Front
========
﻿namespace Shared
>>>>>>>> dd702756306eaf4c4bc0bafadb9b3544b3d89c56:Shared/RequestDto.cs
{
    public class RequestDto
    {
        public ApiType ApiType { get; set; } = ApiType.Get;
        public string Url { get; set; }
        public object? Data { get; set; }
        public string Token { get; set; }
    }
}
