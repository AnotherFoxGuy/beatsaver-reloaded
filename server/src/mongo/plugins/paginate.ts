import {
  AggregatePaginateModel,
  Document,
  PaginateModel,
  PaginateOptions,
} from 'mongoose'
import paginate from 'mongoose-paginate-v2'
import { RESULTS_PER_PAGE } from '../../env'

const pOptions: PaginateOptions = {
  limit: RESULTS_PER_PAGE,
}

interface IPaginateOptions extends PaginateOptions {
  populate: string
}

interface IPaginateResult<T extends Document> {
  docs: T[]
  totalDocs: number

  lastPage: number
  prevPage: number | null
  nextPage: number | null
}

const paginateFn: <D extends Document, M extends PaginateModel<D>>(
  model: M,
  query?: object,
  options?: Partial<IPaginateOptions>
) => Promise<IPaginateResult<D>> = async (model, query, options) => {
  const opts: PaginateOptions = options || {}
  const page = opts.page !== undefined ? opts.page + 1 : undefined

  const {
    docs,
    totalDocs: td,
    totalPages: tp,
    prevPage: p,
    nextPage: n,
  } = await model.paginate(query, { ...opts, page })

  const totalDocs = td as number
  const totalPages = tp as number
  const prev = p as number | null
  const next = n as number | null

  const lastPage = totalPages - 1
  const prevPage = prev === null ? null : prev - 1
  const nextPage = next === null ? null : next - 1

  if (opts.populate) {
    await Promise.all(
      docs.map(d => d.populate(opts.populate as string).execPopulate())
    )
  }

  return { docs, totalDocs, lastPage, prevPage, nextPage }
}

const aggregatePaginateFn: <
  D extends Document,
  M extends AggregatePaginateModel<D>
>(
  model: M,
  aggregateOptions?: any[],
  options?: Partial<IPaginateOptions>
) => Promise<IPaginateResult<D>> = async (model, aggregateOptions, options) => {
  const opts: PaginateOptions = options || {}
  const page = opts.page !== undefined ? opts.page + 1 : undefined

  const aggregate = model.aggregate(aggregateOptions)
  const {
    docs,
    totalDocs: td,
    totalPages: tp,
    prevPage: p,
    nextPage: n,
  } = await model.aggregatePaginate(aggregate, { ...opts, page })

  const totalDocs = td as number
  const totalPages = tp as number
  const prev = p as number | null
  const next = n as number | null

  const lastPage = totalPages - 1
  const prevPage = prev === null ? null : prev - 1
  const nextPage = next === null ? null : next - 1

  if (opts.populate) {
    await Promise.all(
      docs.map(d => d.populate(opts.populate as string).execPopulate())
    )
  }

  return { docs, totalDocs, lastPage, prevPage, nextPage }
}

export { paginateFn as paginate, aggregatePaginateFn as aggregatePaginate }

// @ts-ignore
paginate.paginate.options = pOptions
