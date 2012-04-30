using System.Net;

namespace BabyPics {
  public class PollDaddyRequest {

    protected HttpWebRequest request;

    protected virtual void SetDefaultHeaders() {
      request.Accept = "*/*";
      request.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.3";
      request.Headers["Accept-Encoding"] = "gzip,deflate,sdch";
      request.Headers["Accept-Language"] = "en-US,en;q=0.8";
    }
  }
}
