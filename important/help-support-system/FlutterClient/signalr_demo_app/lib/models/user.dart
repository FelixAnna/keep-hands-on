class User {
  String UserId;
  String TenantId;
  String NickName;
  String AvatarUrl;
  String Email;

  User(
      {required this.UserId,
      required this.TenantId,
      required this.NickName,
      required this.AvatarUrl,
      required this.Email});

  User.fromJson(Map<String, dynamic> json)
      : UserId = json['userId'],
        TenantId = json['tenantId'],
        NickName = json['nickName'],
        AvatarUrl = json['avatarUrl'],
        Email = json['email'];

  Map<String, dynamic> toJson() => {
        'userId': this.UserId,
        'tenantId': this.TenantId,
        'nickName': this.NickName,
        'avatarUrl': this.AvatarUrl,
        'email': this.Email
      };
}
