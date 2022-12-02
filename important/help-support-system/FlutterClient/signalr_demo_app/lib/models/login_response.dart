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
  String UserName;
  String Email;

  User({required this.UserId, required this.UserName, required this.Email});

  User.fromJson(Map<String, dynamic> json)
      : UserId = json['userId'],
        UserName = json['userName'],
        Email = json['email'];

  Map<String, dynamic> toJson() =>
      {'userId': this.UserId, 'userName': this.UserName, 'email': this.Email};
}
