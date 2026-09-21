import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useAuthStore } from '../services/useAuthStore';
import api from '../services/api';
import './Loyalty.css';

interface LoyaltyTx {
  id: string;
  pointsDelta: number;
  reason: string;
  createdAt: string;
}

export default function Loyalty() {
  const { t } = useTranslation();
  const { user } = useAuthStore();
  const [points, setPoints] = useState(user?.loyaltyPoints || 50);
  const [transactions, setTransactions] = useState<LoyaltyTx[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    api.get('/customers/me/loyalty')
      .then((res) => {
        setPoints(res.data?.points || 50);
        setTransactions(res.data?.history || []);
      })
      .catch(() => {
        // Demo fallback transaction history
        setTransactions([
          { id: 'tx-1', pointsDelta: +10, reason: 'Completed Appointment (Hydra-Facial)', createdAt: '2026-09-10' },
          { id: 'tx-2', pointsDelta: +30, reason: 'Welcome Bonus Reward', createdAt: '2026-09-01' },
          { id: 'tx-3', pointsDelta: +10, reason: 'Completed Appointment (Massage)', createdAt: '2026-08-20' },
        ]);
      })
      .finally(() => setLoading(false));
  }, []);

  const progressPercent = Math.min(100, Math.round((points / 100) * 100));

  return (
    <div className="loyalty-page-container">
      <div className="header-banner">
        <h1>{t('Loyalty')}</h1>
        <p className="subtitle">Earn rewards with every appointment you complete.</p>
      </div>

      <div className="loyalty-hero-card">
        <div className="card-top-info">
          <div>
            <h3>{t('Loyalty Points Balance')}</h3>
            <span className="balance-val">{points} <small>{t('Points')}</small></span>
          </div>
          <div className="trophy-icon">🏆</div>
        </div>

        <div className="progress-section">
          <div className="progress-label">
            <span>Reward Status ({progressPercent}%)</span>
            <span>100 Points = Free Spa Service!</span>
          </div>
          <div className="progress-bar-bg">
            <div className="progress-bar-fill" style={{ width: `${progressPercent}%` }} />
          </div>
        </div>

        <div className="loyalty-rules-box">
          <h4>💡 How It Works</h4>
          <ul>
            <li>Earn <strong>10 Points</strong> automatically for every completed reservation.</li>
            <li>Accumulate <strong>100 Points</strong> to unlock a free complimentary treatment.</li>
            <li>Points are automatically added to your profile upon appointment completion.</li>
          </ul>
        </div>
      </div>

      <div className="history-section">
        <h2>Points Transaction History</h2>
        {loading ? (
          <div className="loader">Loading transaction history...</div>
        ) : (
          <div className="tx-table-wrapper">
            <table className="tx-table">
              <thead>
                <tr>
                  <th>Date</th>
                  <th>Activity / Reason</th>
                  <th>Points</th>
                </tr>
              </thead>
              <tbody>
                {transactions.map((tx) => (
                  <tr key={tx.id}>
                    <td>{tx.createdAt}</td>
                    <td>{tx.reason}</td>
                    <td className={tx.pointsDelta >= 0 ? 'pts-earned' : 'pts-redeemed'}>
                      {tx.pointsDelta >= 0 ? `+${tx.pointsDelta}` : tx.pointsDelta} pts
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </div>
  );
}
