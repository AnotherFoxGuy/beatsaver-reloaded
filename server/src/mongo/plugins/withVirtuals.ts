import { Schema } from 'mongoose'

const withVirtuals = (schema: Schema) => {
  const toJSON = schema.get('toJSON')
  schema.set('toJSON', { ...toJSON, virtuals: true, getters: true })
}

export default withVirtuals
