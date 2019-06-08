import Router from 'koa-router'
import { IS_DEV } from '../env'
import signale from '../utils/signale'
import { authRouter } from './auth'
import { cdnRouter } from './cdn'
import { downloadRouter } from './download'
import { mapsRouter } from './maps'
import { searchRouter } from './search'
import { uploadRouter } from './upload'

export const routes: Router[] = [
  authRouter,
  downloadRouter,
  mapsRouter,
  searchRouter,
  uploadRouter,
]

if (IS_DEV) {
  signale.warn('Enabling local CDN... Do not use this in production!')
  routes.push(cdnRouter)
}
