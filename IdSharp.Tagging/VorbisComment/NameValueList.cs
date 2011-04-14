using System.Collections.Generic;

namespace IdSharp.Tagging.VorbisComment
{
    /// <summary>
    /// Name/Value list used by some tagging methods.
    /// </summary>
    public class NameValueList : List<NameValueItem>
    {
        /// <summary>
        /// Gets the first value for a specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public string GetValue(string key)
        {
            foreach (NameValueItem item in this)
            {
                if (string.Compare(item.Name, key, true) == 0)
                    return item.Value;
            }

            return null;
        }

        /// <summary>
        /// Gets all values for a specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public string[] GetAllValues(string key)
        {
            List<string> values = new List<string>();

            foreach (NameValueItem item in this)
            {
                if (string.Compare(item.Name, key, true) == 0)
                    values.Add(item.Value);
            }

            return values.ToArray();
        }

        /// <summary>
        /// Sets the value for a specified key. Removes all existing name/value pairs for this key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void SetValue(string key, string value)
        {
            foreach (NameValueItem item in new List<NameValueItem>(this))
            {
                if (string.Compare(item.Name, key, true) == 0)
                {
                    Remove(item);
                }
            }

            Add(new NameValueItem(key, value));
        }
    }
}
