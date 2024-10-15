using System.Text.RegularExpressions;

namespace Utils.Minio.impl
{
    /// <summary>
    ///    Minio的名称约束处理
    /// </summary>
    public class MinioNameNormalizer : IMinioNameNormalizer
    {
        /// <summary>
        ///     https://docs.aws.amazon.com/AmazonS3/latest/dev/BucketRestrictions.html
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string NormalizeBucketName(string name)
        {
            // All letters in a container name must be lowercase.
            name = name.ToLower();

            // Container names must be from 3 through 63 characters long.
            if (name.Length > 63)
            {
                name = name.Substring(0, 63);
            }

            // Bucket names can consist only of lowercase letters, numbers, dots (.), and hyphens (-).
            name = Regex.Replace(name, "[^a-z0-9-.]", string.Empty);

            // Bucket names must begin and end with a letter or number.
            // Bucket names must not be formatted as an IP address (for example, 192.168.5.4).
            // Bucket names can't start or end with hyphens adjacent to period
            // Bucket names can't start or end with dots adjacent to period
            name = Regex.Replace(name, "\\.{2,}", ".");
            name = Regex.Replace(name, "-\\.", string.Empty);
            name = Regex.Replace(name, "\\.-", string.Empty);
            name = Regex.Replace(name, "^-", string.Empty);
            name = Regex.Replace(name, "-$", string.Empty);
            name = Regex.Replace(name, "^\\.", string.Empty);
            name = Regex.Replace(name, "\\.$", string.Empty);
            name = Regex.Replace(name, "^(?:(?:^|\\.)(?:2(?:5[0-5]|[0-4]\\d)|1?\\d?\\d)){4}$", string.Empty);

            if (name.Length < 3)
            {
                var length = name.Length;
                for (var i = 0; i < 3 - length; i++)
                {
                    name += "0";
                }
            }

            return name;
        }

        /// <summary>
        ///      https://docs.aws.amazon.com/AmazonS3/latest/dev/UsingMetadata.html
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string NormalizeObjectName(string name)
        {
            return name;
        }
    }
}
