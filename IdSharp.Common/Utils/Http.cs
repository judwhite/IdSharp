using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace IdSharp.Common.Utils
{
    /// <summary>
    /// Methods for HTTP
    /// </summary>
    public static class Http
    {
        private const string m_UserAgent = @"Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.9.0.9) Gecko/2009040821 Firefox/3.0.9 (.NET CLR 3.5.30729)";

        private class RequestAndData
        {
            public HttpWebRequest Request;
            public byte[] Data;
        }

        /// <summary>
        /// GETs a byte array of data from the specified request URI string.
        /// </summary>
        /// <param name="requestUriString">The request URI string.</param>
        /// <param name="postData">The post data.</param>
        /// <returns>A byte array of data from the specified request URI string.</returns>
        public static byte[] Get(string requestUriString, IEnumerable<PostData> postData)
        {
            if (string.IsNullOrWhiteSpace(requestUriString))
                throw new ArgumentNullException("requestUriString");
            if (postData == null)
                throw new ArgumentNullException("postData");

            return Get(GetQueryString(requestUriString, postData));
        }

        /// <summary>
        /// GETs a byte array of data from the specified request URI string.
        /// </summary>
        /// <param name="requestUriString">The request URI string.</param>
        /// <returns>A byte array of data from the specified request URI string.</returns>
        public static byte[] Get(string requestUriString)
        {
            return Get(requestUriString, cookies: null, credentials: null);
        }

        /// <summary>
        /// GETs a byte array of data from the specified request URI string.
        /// </summary>
        /// <param name="requestUriString">The request URI string.</param>
        /// <param name="credentials">The Basic Authentication credentials.</param>
        /// <returns>A byte array of data from the specified request URI string.</returns>
        public static byte[] Get(string requestUriString, BasicAuthenticationCredentials credentials)
        {
            return Get(requestUriString, cookies: null, credentials: credentials);
        }

        /// <summary>
        /// GETs a byte array of data from the specified request URI string.
        /// </summary>
        /// <param name="requestUriString">The request URI string.</param>
        /// <param name="cookies">The cookies.</param>
        /// <param name="credentials">The Basic Authentication credentials.</param>
        /// <returns>A byte array of data from the specified request URI string.</returns>
        public static byte[] Get(string requestUriString, CookieContainer cookies, BasicAuthenticationCredentials credentials)
        {
            if (string.IsNullOrWhiteSpace(requestUriString))
                throw new ArgumentNullException("requestUriString");

            HttpWebResponse webResponse;
            return Get(requestUriString, cookies, credentials, out webResponse);
        }

        /// <summary>
        /// GETs a byte array of data from the specified request URI string.
        /// </summary>
        /// <param name="requestUriString">The request URI string.</param>
        /// <param name="cookies">The cookies.</param>
        /// <param name="credentials">The Basic Authentication credentials.</param>
        /// <param name="webResponse">The web response.</param>
        /// <returns>A byte array of data from the specified request URI string.</returns>
        public static byte[] Get(string requestUriString, CookieContainer cookies, BasicAuthenticationCredentials credentials, out HttpWebResponse webResponse)
        {
            if (string.IsNullOrWhiteSpace(requestUriString))
                throw new ArgumentNullException("requestUriString");

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
            webRequest.Proxy = WebRequest.DefaultWebProxy;
            webRequest.CookieContainer = cookies;
            webRequest.UserAgent = m_UserAgent;
            if (credentials != null)
            {
                webRequest.Headers.Add(string.Format("Authorization: Basic {0}", Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", credentials.UserName, credentials.Password)))));
            }
            webResponse = (HttpWebResponse)webRequest.GetResponse();

            byte[] data = new byte[256];
            using (Stream responseStream = webResponse.GetResponseStream())
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    int read;
                    do
                    {
                        read = responseStream.Read(data, 0, 256);
                        memoryStream.Write(data, 0, read);
                    } while (read > 0);

                    data = memoryStream.ToArray();
                }
            }

            return data;
        }

        /// <summary>
        /// POSTs to the specified request URI string asynchronously.
        /// </summary>
        /// <param name="requestUriString">The request URI string.</param>
        /// <param name="postData">The post data.</param>
        /// <returns>An <see cref="IAsyncResult"/> that references the asynchronous request.</returns>
        public static IAsyncResult PostAsync(string requestUriString, IEnumerable<PostData> postData)
        {
            if (string.IsNullOrWhiteSpace(requestUriString))
                throw new ArgumentNullException("requestUriString");
            if (postData == null)
                throw new ArgumentNullException("postData");

            byte[] data;
            HttpWebRequest webRequest = GetPostRequest(requestUriString, postData, out data);

            RequestAndData requestAndData = new RequestAndData { Request = webRequest, Data = data };

            return webRequest.BeginGetRequestStream(GetRequestStreamCallback, requestAndData);
        }

        /// <summary>
        /// POSTs to the specified request URI string.
        /// </summary>
        /// <param name="requestUriString">The request URI string.</param>
        /// <param name="postData">The post data.</param>
        /// <param name="cookies">The cookies.</param>
        public static byte[] Post(string requestUriString, IEnumerable<PostData> postData, out CookieContainer cookies)
        {
            if (string.IsNullOrWhiteSpace(requestUriString))
                throw new ArgumentNullException("requestUriString");
            if (postData == null)
                throw new ArgumentNullException("postData");

            Uri uri = new Uri(requestUriString);
            return Post(uri, postData, out cookies);
        }

        /// <summary>
        /// POSTs to the specified request URI string.
        /// </summary>
        /// <param name="requestUriString">The request URI string.</param>
        /// <param name="postData">The post data.</param>
        /// <param name="cookies">The cookies.</param>
        public static byte[] PostCookies(string requestUriString, IEnumerable<PostData> postData, CookieContainer cookies)
        {
            if (string.IsNullOrWhiteSpace(requestUriString))
                throw new ArgumentNullException("requestUriString");
            if (postData == null)
                throw new ArgumentNullException("postData");

            Uri uri = new Uri(requestUriString);
            return PostCookies(uri, postData, cookies);
        }

        /// <summary>
        /// POSTs to the specified request URI string.
        /// </summary>
        /// <param name="requestUriString">The request URI string.</param>
        /// <param name="postData">The post data.</param>
        public static byte[] Post(string requestUriString, IEnumerable<PostData> postData)
        {
            if (string.IsNullOrWhiteSpace(requestUriString))
                throw new ArgumentNullException("requestUriString");
            if (postData == null)
                throw new ArgumentNullException("postData");

            Uri uri = new Uri(requestUriString);
            CookieContainer cookies;
            return Post(uri, postData, out cookies);
        }

        /// <summary>
        /// POSTs to the specified request URI.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        public static byte[] Post(Uri requestUri)
        {
            if (requestUri == null)
                throw new ArgumentNullException("requestUri");

            CookieContainer cookies;
            return Post(requestUri, null, out cookies);
        }

        /// <summary>
        /// POSTs to the specified request URI.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="postData">The post data.</param>
        /// <param name="cookies">The cookies.</param>
        public static byte[] Post(Uri requestUri, IEnumerable<PostData> postData, out CookieContainer cookies)
        {
            if (requestUri == null)
                throw new ArgumentNullException("requestUri");

            byte[] data;
            HttpWebRequest webRequest = GetPostRequest(requestUri, postData, out data);

            using (Stream postStream = webRequest.GetRequestStream())
            {
                postStream.Write(data, 0, data.Length);
            }

            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

            using (Stream responseStream = webResponse.GetResponseStream())
            {
                data = new byte[256];
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    int read;
                    do
                    {
                        read = responseStream.Read(data, 0, 256);
                        memoryStream.Write(data, 0, read);
                    } while (read > 0);

                    data = memoryStream.ToArray();
                }
            }

            cookies = webRequest.CookieContainer;

            return data;
        }

        /// <summary>
        /// POSTs to the specified request URI.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="postData">The post data.</param>
        /// <param name="cookies">The cookies.</param>
        public static byte[] PostCookies(Uri requestUri, IEnumerable<PostData> postData, CookieContainer cookies)
        {
            if (requestUri == null)
                throw new ArgumentNullException("requestUri");

            byte[] data;
            HttpWebRequest webRequest = GetPostRequest(requestUri, postData, out data);
            webRequest.CookieContainer = cookies;

            using (Stream postStream = webRequest.GetRequestStream())
            {
                postStream.Write(data, 0, data.Length);
            }

            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

            using (Stream responseStream = webResponse.GetResponseStream())
            {
                data = new byte[256];
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    int read;
                    do
                    {
                        read = responseStream.Read(data, 0, 256);
                        memoryStream.Write(data, 0, read);
                    } while (read > 0);

                    data = memoryStream.ToArray();
                }
            }

            return data;
        }

        /// <summary>
        /// Gets the full URI including GET/POST arguments.
        /// </summary>
        /// <param name="requestUriString">The request URI string.</param>
        /// <param name="postData">The post data.</param>
        public static string GetQueryString(string requestUriString, IEnumerable<PostData> postData)
        {
            if (string.IsNullOrWhiteSpace(requestUriString))
                throw new ArgumentNullException("requestUriString");
            if (postData == null)
                throw new ArgumentNullException("postData");

            StringBuilder getString = new StringBuilder();
            foreach (PostData item in postData)
                AddField(getString, item);

            if (!requestUriString.Contains("?"))
                getString.Insert(0, '?');
            else
                getString.Insert(0, '&');

            return string.Format("{0}{1}", requestUriString, getString);
        }

        private static void AddField(StringBuilder postString, PostData postData)
        {
            if (postString == null)
                throw new ArgumentNullException("postString");
            if (postData == null)
                throw new ArgumentNullException("postData");
            if (postData.Value == null)
                throw new ArgumentNullException("postData.Item");

            StringBuilder value = new StringBuilder();
            foreach (char c in postData.Value)
            {
                if (c <= 255)
                {
                    if (c < 128 && c != '&' && c != ' ')
                    {
                        value.Append(c);
                    }
                    else
                    {
                        value.Append(string.Format("%{0:X2}", (int)c));
                    }
                }
            }


            if (postString.Length == 0)
                postString.AppendFormat("{0}={1}", postData.Field, value);
            else
                postString.AppendFormat("&{0}={1}", postData.Field, value);
        }

        private static HttpWebRequest GetPostRequest(string requestUriString, IEnumerable<PostData> postData, out byte[] data)
        {
            if (string.IsNullOrWhiteSpace(requestUriString))
                throw new ArgumentNullException("requestUriString");
            if (postData == null)
                throw new ArgumentNullException("postData");

            Uri uri = new Uri(requestUriString);
            return GetPostRequest(uri, postData, out data);
        }

        private static HttpWebRequest GetPostRequest(Uri requestUri, IEnumerable<PostData> postData, out byte[] data)
        {
            if (requestUri == null)
                throw new ArgumentNullException("requestUri");

            StringBuilder getString = new StringBuilder();
            foreach (PostData item in postData)
                AddField(getString, item);

            data = Encoding.ASCII.GetBytes(getString.ToString());

            // Prepare web request...
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(requestUri);
            webRequest.AllowAutoRedirect = true;
            webRequest.Proxy = WebRequest.DefaultWebProxy;
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = data.Length;
            webRequest.CookieContainer = new CookieContainer();
            webRequest.UserAgent = m_UserAgent;

            return webRequest;
        }

        private static void GetRequestStreamCallback(IAsyncResult asyncResult)
        {
            RequestAndData requestAndData = (RequestAndData)asyncResult.AsyncState;
            HttpWebRequest webRequest = requestAndData.Request;
            byte[] data = requestAndData.Data;

            using (Stream postStream = webRequest.EndGetRequestStream(asyncResult))
            {
                postStream.Write(data, 0, data.Length);
            }
        }

        /// <summary>
        /// Gets the file name from headers.
        /// </summary>
        /// <param name="headers">The headers.</param>
        /// <returns>The file name.</returns>
        public static string GetFileNameFromHeaders(WebHeaderCollection headers)
        {
            if (headers == null)
                throw new ArgumentNullException("headers");

            string[] keys = headers.AllKeys;
            if (keys == null || keys.Length == 0)
                throw new Exception("No keys found.");

            List<string> keyList = new List<string>(keys);
            if (keyList.Contains("Content-disposition"))
            {
                string contentDisposition = headers["Content-disposition"];

                const string filename = "filename=\"";
                int idx = contentDisposition.IndexOf(filename);
                idx = idx + filename.Length;
                string fn = contentDisposition.Substring(idx, contentDisposition.Length - idx - 1);

                return fn;
            }

            throw new Exception("'Content-disposition' header not found.");
        }
    }

    /// <summary>
    /// HTTP Basic Authentication credentials.
    /// </summary>
    public class BasicAuthenticationCredentials
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicAuthenticationCredentials"/> class.
        /// </summary>
        public BasicAuthenticationCredentials()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicAuthenticationCredentials"/> class.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="password">The password.</param>
        public BasicAuthenticationCredentials(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        /// <value>The user name.</value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password { get; set; }
    }
}