import jwt from 'jsonwebtoken'
import passport from 'passport'
import { ExtractJwt, Strategy as JWTStrategy } from 'passport-jwt'
import { JWT_SECRET } from '../env'
import User, { IUserModel } from '../mongo/models/User'

export interface IAuthToken {
  _id: string
  username: string
  admin: boolean
}

passport.use(
  new JWTStrategy(
    {
      jwtFromRequest: ExtractJwt.fromAuthHeaderAsBearerToken(),
      secretOrKey: JWT_SECRET,
    },
    async (payload: IAuthToken, cb) => {
      try {
        const user = await User.findById(payload._id)
        if (!user) return cb(null, false)

        return cb(null, user)
      } catch (err) {
        return cb(err)
      }
    }
  )
)

export const issueToken: (user: IUserModel) => Promise<string> = user =>
  new Promise((resolve, reject) => {
    const payload: IAuthToken = {
      _id: user._id,
      admin: user.admin,
      username: user.username,
    }

    jwt.sign(payload, JWT_SECRET, { expiresIn: '7 days' }, (err, token) => {
      if (err) return reject(err)
      else return resolve(token)
    })
  })
