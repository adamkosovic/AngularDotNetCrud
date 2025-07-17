# 🚀 Deploying BookApp to Netlify

This guide will help you deploy your Angular frontend to Netlify while keeping your .NET backend separate.

## 📋 Prerequisites

- [Netlify account](https://netlify.com)
- [GitHub account](https://github.com)
- Your .NET backend deployed (Azure, Heroku, Railway, etc.)

## 🔧 Step 1: Prepare Your Backend

Before deploying the frontend, you need to deploy your .NET backend:

### Option A: Deploy to Azure

```bash
# Install Azure CLI
az login
az webapp up --name your-bookapp-api --resource-group your-resource-group
```

### Option B: Deploy to Railway

1. Go to [Railway.app](https://railway.app)
2. Connect your GitHub repository
3. Deploy the .NET backend
4. Get the production URL

### Option C: Deploy to Heroku

```bash
# Install Heroku CLI
heroku create your-bookapp-api
git push heroku main
```

## 🌐 Step 2: Update Production API URL

Once your backend is deployed, update the production environment:

1. Edit `book-app/src/environments/environment.prod.ts`
2. Replace `'https://your-backend-url.com'` with your actual backend URL
3. Example: `'https://your-bookapp-api.azurewebsites.net'`

## 📦 Step 3: Deploy to Netlify

### Method A: Drag & Drop (Quick)

1. **Build your Angular app:**

   ```bash
   cd bin/Debug/net9.0/book-app
   npm run build
   ```

2. **Go to Netlify:**

   - Visit [netlify.com](https://netlify.com)
   - Sign up/Login
   - Click "Add new site" → "Deploy manually"

3. **Upload the build folder:**
   - Drag the `dist/book-app` folder to the deploy area
   - Your site will be live in seconds!

### Method B: Git Integration (Recommended)

1. **Push your code to GitHub:**

   ```bash
   git add .
   git commit -m "Prepare for Netlify deployment"
   git push origin main
   ```

2. **Connect to Netlify:**

   - Go to [netlify.com](https://netlify.com)
   - Click "Add new site" → "Import an existing project"
   - Connect your GitHub account
   - Select your repository

3. **Configure build settings:**

   - **Build command:** `cd bin/Debug/net9.0/book-app && npm run build`
   - **Publish directory:** `bin/Debug/net9.0/book-app/dist/book-app`
   - **Node version:** 18

4. **Deploy:**
   - Click "Deploy site"
   - Netlify will build and deploy automatically

## ⚙️ Step 4: Configure Environment Variables

In your Netlify dashboard:

1. Go to **Site settings** → **Environment variables**
2. Add:
   - `NODE_VERSION`: `18`
   - `API_URL`: Your backend URL

## 🔄 Step 5: Set Up Continuous Deployment

Netlify will automatically:

- ✅ Deploy when you push to `main` branch
- ✅ Build your Angular app
- ✅ Serve it with HTTPS
- ✅ Provide a custom domain

## 🌍 Step 6: Custom Domain (Optional)

1. In Netlify dashboard, go to **Domain settings**
2. Click **Add custom domain**
3. Follow the DNS configuration instructions

## 🐛 Troubleshooting

### Build Errors

```bash
# Clear cache and rebuild
cd bin/Debug/net9.0/book-app
rm -rf node_modules
npm install
npm run build
```

### CORS Issues

Make sure your backend allows requests from your Netlify domain:

```csharp
// In Program.cs
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNetlify",
        policy => policy
         dotnet publish -c Release
      .WithOrigins("https://rad-strudel-8320dd.netlify.app")
            .AllowAnyHeader()
            .AllowAnyMethod());
});
```

### Environment Issues

- Check that `environment.prod.ts` has the correct API URL
- Verify environment variables in Netlify dashboard

## 📱 Your App is Live!

Once deployed, your app will be available at:

- **Netlify URL:** `https://your-app-name.netlify.app`
- **Custom Domain:** `https://yourdomain.com` (if configured)

## 🔄 Updating Your App

1. Make changes to your code
2. Push to GitHub
3. Netlify automatically rebuilds and deploys
4. Your changes are live in minutes!

---

**🎉 Congratulations! Your BookApp is now live on the web!**
