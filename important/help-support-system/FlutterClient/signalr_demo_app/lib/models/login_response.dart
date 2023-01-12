class LoginResponse {
  String AccessToken;
  User Profile;
  LoginResponse({required this.AccessToken, required this.Profile});

  LoginResponse.fromJson(Map<String, dynamic> json)
      : AccessToken = json['accessToken'],
        Profile = json['profile'];

  Map<String, dynamic> toJson() =>
      {'accessToken': this.AccessToken, 'profile': this.Profile};
}

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
