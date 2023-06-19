# LiveMedy Case

### Installation
Type to package manager console to update or create SQLite database
```powershell
update-database
```

### Keynotes
Admin login infos
```powershell
username: admin
password: admin
```

- **ReCaptcha verification on signIn and signUp
- **Rate Limiting on signUp
- **Admin and other roles have separate dashboard
- **Users have own referral link to share on dashboard. Below is the example link
```powershell
https://localhost:7117/Auth/SignUp?referralcode=XXXX
```

