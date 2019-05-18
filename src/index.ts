import cors from '@koa/cors'
import Koa from 'koa'
import helmet from 'koa-helmet'
import Router from 'koa-router'
import mongoose from 'mongoose'
import { IS_DEV, MONGO_URL, PORT } from './env'
import { logger } from './middleware'
import { errorHandler } from './middleware/errors'
import { apiRouter, authRouter } from './routes'
import './strategies'
import signale, { panic } from './utils/signale'

export const app = new Koa()
const router = new Router()

if (!IS_DEV) {
  app.proxy = true
} else {
  app.use(cors({ exposeHeaders: ['x-auth-token'] }))
}

app
  .use(helmet({ hsts: false }))
  .use(logger)
  .use(errorHandler)

const registerRoutes = (first: Router | Router[], ...routes: Router[]) => {
  const rs = Array.isArray(first) ? [...first, ...routes] : [first, ...routes]
  rs.forEach(r => router.use(r.routes(), r.allowedMethods()))

  app.use(router.routes()).use(router.allowedMethods())
}

if (IS_DEV) signale.warn('Running in development environment!')
mongoose.set('useCreateIndex', true)
mongoose
  .connect(MONGO_URL, { useNewUrlParser: true })
  .then(async () => {
    signale.info(`Connected to MongoDB ${IS_DEV ? 'Instance' : 'Cluster'}`)

    registerRoutes(apiRouter, authRouter)
    app.listen(PORT).on('listening', () => {
      signale.start(`Listening over HTTP on port ${PORT}`)
    })
  })
  .catch(err => {
    signale.fatal(
      `Failed to connect to MongoDB ${IS_DEV ? 'Instance' : 'Cluster'}!`
    )

    return panic(err)
  })
