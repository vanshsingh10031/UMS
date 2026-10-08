# Free Deployment: Render API + Vercel Frontend

## 1. Backend on Render

Create a new Render **Web Service** from this repo and use the included `render.yaml`.

Set these environment variables in Render:

```txt
ConnectionStrings__DefaultConnection=Server=161.248.26.41,1533;Database=UMSPresentationReplica;User Id=Dynode_SA;Password=<your sql password>;TrustServerCertificate=True;Encrypt=True;MultipleActiveResultSets=True
Jwt__Key=<any strong 32+ character key>
Jwt__Issuer=UMS
Jwt__Audience=UMS-Frontend
CORS_ORIGINS=https://<your-vercel-app>.vercel.app,http://localhost:5173,http://127.0.0.1:5173
ASPNETCORE_ENVIRONMENT=Production
```

After deploy, test:

```txt
https://<your-render-service>.onrender.com/api/health
```

## 2. Frontend on Vercel

Create a Vercel project from this repo.

Set this environment variable in Vercel:

```txt
VITE_API_BASE_URL=https://<your-render-service>.onrender.com/api
```

Build settings:

```txt
Framework Preset: Vite
Build Command: npm run build
Output Directory: dist
```

## 3. Update CORS After Vercel URL Is Known

Once Vercel gives the final URL, go back to Render and update:

```txt
CORS_ORIGINS=https://<your-vercel-app>.vercel.app,http://localhost:5173,http://127.0.0.1:5173
```

Then redeploy/restart the Render service.

## 4. Presentation Fallback

If Render sleeps or starts slowly, use the local backend:

```txt
cd Backend/UMS.Api
dotnet run --urls http://localhost:5052
```

Then run the frontend locally:

```txt
npm run dev
```
