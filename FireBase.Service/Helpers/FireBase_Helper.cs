namespace FireBase.Service
{
    public class FireBase_Helper
    {
        private readonly string _privateKey;

        public FireBase_Helper()
        {
            // I use it like this and not in file, so we can push it in Github.
            _privateKey = @"{
              ""type"": ""service_account"",
              ""project_id"": ""busfirebaseproject-c4384"",
              ""private_key_id"": ""c0ab18c37d5b18a08b64e74e4465e789c65c22cb"",
              ""private_key"": ""-----BEGIN PRIVATE KEY-----\nMIIEvwIBADANBgkqhkiG9w0BAQEFAASCBKkwggSlAgEAAoIBAQCwjCGFdAEu76d7\n6KNQ0kr8ntG6J38aNKCrcrxrb+V5yJmbUhL8Bm+yzjVHeV0LMZ75G5SpuFtSMjTq\nbnvzjgHIkknAD6fHQxGA26p6hTHqxo+LUWkbMm/f28Ylnjcb/+Azhfe0/WA83pAs\nsmWmZ7CtxUkte3pYgb1v4V31rVsThVgx1q7qZzF0eOlzGDzo4ReCZ/gFeSlJTsaP\n2vS0plLvDuZP3JfzaAHMkO7W3lBrs6ClM5bwI69DXohEcp6zJMDjOYPEY+kx3OOC\ncTj6Qfas56DGSiL9/YcnwyJFLETxntiGvbq2F7c/O9ptR8QmYIiKolaPqFzwXIgk\nPmfeHFixAgMBAAECggEAC4bUq/TJXH4FbH3BhvX58yEv7oRkZCq+OD9tHaMgQ4ml\nPMq4pleJvK4BtMVP3bqbR2ald8jLfpP8WDeHNH1HsMebVNS41rXSa1w8NtRiMFLP\nPbB+qSnLe91KpDtem/+CbP7Az+JwoXzDA0YRBLK1qnzMZLGwiN2Mp6S6WJ9dSEqD\nVzd8IsJ8u6MPvvXh/kdEhPjljFdJdVAzQ4LF9dNcGf+CCXNgCYlmAn6GOFF7HvTC\nUD5dsafYSHJ4t/5fcObXOyEWpEOBiBoucettRo/0Bg3g5Y3lnW7BdAX0yJdfpXrb\nhLQlE1/FM8M45n9ii3GuqYTxMWZ3rY5DzYFQbvJ4uQKBgQDVKTsBnJlZHE8j6Yne\nO8XApm2DM3RhhjgLA+jv+gFNGLjvmGOjb3R61Xn49PHkRSqeP2map63hzpm5SpcH\n/tdC1PgDaAN5hiaxN/I4Rr7NttAxBM8yVPLQj9H2eRgccuMLd4mFHiYF7FlLDX/i\nGbOQYHTudelbMggU09RhA1momQKBgQDUBzCsqybwKANmAR3GV6fYKjeOJrGbnCF5\niQ5yRQwB62OUhm7eApccgZlMt+8iI3nk2XjkKcurBaqZ3XS/MxAnB2x6zFKRc90c\nvTXnFnDHqgwpaJoX4X2XZgcf9pPkt8AOcti24ejCN0k3b2EI5Wgq2aJSZk1ss3eG\nrM4nampH2QKBgQDUB3gXKVLr7ZnXCgVGVkRlVeaW3AbGv0BdzJzWn+LNcmr6gKWT\nu1ismk4CUnuN6TL0R/Veja4Lb3s/cS98F4S9iKPOl5blOYihPRol2lEOxOqf573F\nvfPtezGJrnhKTAkFrfjPveZqe+dyHVJ4b8JKOXf2frkJKmIZfX/CpBd5+QKBgQCB\n5EnAbjWziH/UfLiV6ttLhUl1j2TdNMAZKYqtHHmnr2HY+qZu4d1bPFYC9ufL3Tnu\npf/2n1hDVHxYOKAuBgVOM7EUZZnru7Rox81+3XOIDjfXIlrMaHq9Rmb1AOVHh/5j\nm54CI9GpMZ9sE5K5lXjET4GuwzeJcUS3P6Qn53xmuQKBgQCu6/nVwizckXd/8PSY\nlYA+cjbeNcoYihAgoqUwfT8+dBujsxfyb7QbV/diDKlKS0R3H9HQtjKiW7XueyyB\nmqLX5WmTCzyiZAww05c/diiT6EbGzIbgEHF6n9RHz7b9VvPYbvPhQeSOI7W77yqX\nySeBZptOhrNn+tPXQ3TqqiEaEw==\n-----END PRIVATE KEY-----\n"",
              ""client_email"": ""firebase-adminsdk-4kl3e@busfirebaseproject-c4384.iam.gserviceaccount.com"",
              ""client_id"": ""110659733056204000730"",
              ""auth_uri"": ""https://accounts.google.com/o/oauth2/auth"",
              ""token_uri"": ""https://oauth2.googleapis.com/token"",
              ""auth_provider_x509_cert_url"": ""https://www.googleapis.com/oauth2/v1/certs"",
              ""client_x509_cert_url"": ""https://www.googleapis.com/robot/v1/metadata/x509/firebase-adminsdk-4kl3e%40busfirebaseproject-c4384.iam.gserviceaccount.com"",
              ""universe_domain"": ""googleapis.com""
            }";
        }
        public static FireBase_Helper Instance { get; } = new FireBase_Helper();

        public string GetPrivateKey() => _privateKey;
    }
}