class User {
  String UserId;
  String NickName;
  String AvatarUrl;
  String Email;

  User(
      {required this.UserId,
      required this.NickName,
      required this.AvatarUrl,
      required this.Email});

  User.fromJson(Map<String, dynamic> json)
      : UserId = json['userId'],
        NickName = json['nickName'],
        AvatarUrl = json['avatarUrl'],
        Email = json['email'];

  Map<String, dynamic> toJson() => {
        'userId': this.UserId,
        'nickName': this.NickName,
        'avatarUrl': this.AvatarUrl,
        'email': this.Email
      };
}
