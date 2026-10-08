export default function Brand({ compact = false }) {
  return (
    <div className={`brand ${compact ? 'brand--compact' : ''}`}>
      <div className="brand__mark"><span>अ</span></div>
      <div>
        <strong>AMRITA</strong>
        {!compact && <small>VISHWA VIDYAPEETHAM</small>}
      </div>
    </div>
  )
}
